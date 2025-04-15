const $ = id => document.getElementById(id);

let recordatoriosSeleccionados = [];
let comentariosSeleccionados = [];

function toggleFormulario() {
    $("formularioTarea").classList.toggle("activo");
}

function validarFormulario() {
    let isValid = true;

    const campos = [
        { id: "tituloTarea", mensaje: "El título es obligatorio." },
        { id: "descripcionTarea", mensaje: "La descripción es obligatoria." },
        { id: "fechaLimite", mensaje: "La fecha límite es obligatoria." },
        { id: "estadoTarea", mensaje: "El estado es obligatorio." },
        { id: "prioridadTarea", mensaje: "La prioridad es obligatoria." },
    ];

    campos.forEach(campo => {
        const input = document.getElementById(campo.id);
        if (!input.value.trim()) {
            input.classList.add("is-invalid");
            const feedback = input.nextElementSibling;
            if (feedback) feedback.textContent = campo.mensaje;
            isValid = false;
        } else {
            input.classList.remove("is-invalid");
            input.classList.add("is-valid");
        }
    });

    return isValid;
}

async function mostrarComentarios(comentarios) {
    const contenedor = document.getElementById("zona-comentarios");
    contenedor.innerHTML = "";

    if (comentarios.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">No hay comentarios aún.</p>`;
        return;
    }

    const response = await fetch("/Tarea/GetUsuarios");
    const usuarios = await response.json();

    comentarios.forEach(comentario => {
        const usuario = usuarios.find(u => u.idUsuario === comentario.idUsuario);
        const nombreUsuario = usuario ? usuario.nombre : "Anónimo";

        const div = document.createElement("div");
        div.className = "mb-2";
        div.innerHTML = `
            <p class="mb-0"><strong>${nombreUsuario}:</strong> ${comentario.texto}</p>
            <small class="text-muted">${new Date(comentario.fechaHora).toLocaleString()}</small>
        `;
        contenedor.appendChild(div);
    });

    contenedor.scrollTop = contenedor.scrollHeight;
}

function mostrarRecordatoriosDeTarea(recordatorios) {
    const contenedor = $("listaRecordatorios");
    contenedor.innerHTML = "";

    if (recordatorios.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">No hay recordatorios aún.</p>`;
        return;
    }

    recordatorios.forEach(r => {
        const div = document.createElement("div");
        div.className = "mb-2";

        const fecha = new Date(r.fechaHora);
        const fechaStr = fecha.toLocaleString();

        div.innerHTML = `
            <p class="mb-0"><strong>Recordatorio:</strong> ${r.Mensaje}</p>
            <small class="text-muted">${fechaStr}</small>
        `;

        contenedor.appendChild(div);
    });

    contenedor.scrollTop = contenedor.scrollHeight;
}

document.addEventListener("click", ({ target }) => {
    const modal = $("formularioTarea");
    const btnAbrir = $("btnAbrirFormulario");

    if (modal.classList.contains("activo") &&
        !modal.contains(target) &&
        !btnAbrir.contains(target)) {
        modal.classList.remove("activo");
    }
});

document.addEventListener("keydown", ({ key }) => {
    if (key === "Escape") {
        $("formularioTarea").classList.remove("activo");
    }
});

async function cargarTarea(id) {
    try {
        const response = await fetch(`/Tarea/Details/${id}`);
        const tarea = await response.json();

        setFieldValues({
            tituloTarea: tarea.titulo,
            descripcionTarea: tarea.descripcion,
            fechaLimite: formatDateTimeForInput(tarea.fechaLimite),
            estadoTarea: tarea.estado,
            prioridadTarea: tarea.prioridad,
            idTarea: tarea.idTarea,
            idUsuario: tarea.idUsuario,
            idEquipo: tarea.idEquipo || ''
        });

        recordatoriosSeleccionados = tarea.recordatoriosList || [];
        mostrarRecordatorios(recordatoriosSeleccionados);

        comentariosSeleccionados = tarea.comentariosList || [];
        mostrarComentarios(comentariosSeleccionados);

        toggleFormulario();
    } catch (error) {
        console.error("Error al cargar la tarea:", error);
        alert("Error al cargar la tarea. Por favor intente nuevamente.");
    }
}

function formatDateTimeForInput(dateTimeString) {
    if (!dateTimeString) return '';
    const date = new Date(dateTimeString);
    return date.toISOString().slice(0, 16);
}

function agregarTareaModal() {
    setFieldValues({
        tituloTarea: '',
        descripcionTarea: '',
        fechaLimite: '',
        estadoTarea: '',
        prioridadTarea: '',
        idTarea: '',
        idEquipo: ''
    });

    recordatoriosSeleccionados = [];
    comentariosSeleccionados = [];

    mostrarRecordatorios(recordatoriosSeleccionados);
    mostrarComentarios(comentariosSeleccionados);

    toggleFormulario();
}

function setFieldValues(values) {
    for (const [id, value] of Object.entries(values)) {
        const element = $(id);
        if (element) element.value = value;
    }
}

async function guardarTarea() {
    if (!validarFormulario()) {
        return;
    }
    const getValue = id => document.getElementById(id)?.value.trim();
    const idTarea = getValue("idTarea");

    const titulo = getValue("tituloTarea");
    if (!titulo) {
        alert("El título de la tarea es requerido");
        return;
    }

    const tarea = {
        IdTarea: idTarea ? parseInt(idTarea) : 0,
        IdUsuario: parseInt(getValue("idUsuario")),
        Titulo: titulo,
        Descripcion: getValue("descripcionTarea"),
        FechaLimite: getValue("fechaLimite"),
        Estado: getValue("estadoTarea"),
        Prioridad: getValue("prioridadTarea"),
        IdEquipo: getValue("idEquipo") || null,
        RecordatoriosList: recordatoriosSeleccionados.map(r => ({
            Mensaje: r.mensaje,
            FechaHora: r.fechaHora,
            IdUsuario: r.idUsuario,
            IdTarea: idTarea ? parseInt(idTarea) : 0
        })),
        ComentariosList: comentariosSeleccionados.map(c => ({
            Texto: c.texto,
            IdTarea: idTarea ? parseInt(idTarea) : 0,
            IdUsuario: parseInt(getValue("idUsuario")),
            FechaHora: new Date().toISOString()
        }))
    };

    console.log("Enviando tarea:", tarea);

    const token = sessionStorage.getItem("Token");
    const url = idTarea ? '/Tarea/Edit' : '/Tarea/Create';
    const method = idTarea ? 'PUT' : 'POST';

    try {
        const response = await fetch(url, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(tarea)
        });

        const result = await response.json();
        console.log("Respuesta del servidor:", result);

        if (!response.ok) {
            throw new Error(result.Message || `Error: ${response.statusText}`);
        }

        toggleFormulario();
        location.reload();
    } catch (error) {
        console.error('Error al guardar la tarea:', error);
        alert(error.message || 'Hubo un error al guardar la tarea. Ver la consola para más detalles.');
    }
}

function enviarComentario() {
    const input = document.getElementById("comentarioInput");
    const texto = input.value.trim();

    if (texto) {

        const nuevoComentario = {
            texto: texto,
            fechaHora: new Date().toISOString(),
            idUsuario: parseInt(document.getElementById("idUsuario").value)
        };

        comentariosSeleccionados.push(nuevoComentario);
        mostrarComentarios(comentariosSeleccionados);
        input.value = '';

        const contenedor = document.getElementById("zona-comentarios");
        contenedor.scrollTop = contenedor.scrollHeight;
    }
}

function eliminarRecordatorio(id) {
    recordatoriosSeleccionados = recordatoriosSeleccionados.filter(r => r.idRecordatorio !== id);
    mostrarRecordatoriosDeTarea(recordatoriosSeleccionados);
}

function agregarRecordatorioATarea() {
    const mensajeInput = document.getElementById("mensajeRecordatorio");
    const fechaHoraInput = document.getElementById("fechaRecordatorio");

    const mensaje = mensajeInput.value;
    const fechaHora = fechaHoraInput.value;

    if (!mensaje) {
        mensajeInput.classList.add("is-invalid");
        mensajeInput.nextElementSibling.textContent = "El mensaje es obligatorio.";
        return;
    } else {
        mensajeInput.classList.remove("is-invalid");
    }

    if (!fechaHora) {
        fechaHoraInput.classList.add("is-invalid");
        fechaHoraInput.nextElementSibling.textContent = "La fecha es obligatoria.";
        return;
    } else {
        fechaHoraInput.classList.remove("is-invalid");
    }

    const nuevoRecordatorio = {
        idRecordatorio: Date.now(), 
        mensaje: mensaje,
        fechaHora: fechaHora,
        idUsuario: parseInt(document.getElementById("idUsuario").value)
    };

    recordatoriosSeleccionados.push(nuevoRecordatorio);
    mostrarRecordatorios(recordatoriosSeleccionados);

    mensajeInput.value = "";
    fechaHoraInput.value = "";
    mensajeInput.focus();
}


function eliminarRecordatorio(idRecordatorio, event) {
    if (event) event.stopPropagation();

    recordatoriosSeleccionados = recordatoriosSeleccionados.filter(r => r.idRecordatorio !== idRecordatorio);
    mostrarRecordatorios(recordatoriosSeleccionados);
}


function mostrarRecordatorios(recordatorios) {
    const contenedor = document.getElementById("listaRecordatorios");
    contenedor.innerHTML = "";

    if (recordatorios.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">No hay recordatorios aún.</p>`;
        return;
    }

    recordatorios.forEach(r => {
        const div = document.createElement("div");
        div.className = "d-flex align-items-center bg-participante justify-content-between p-2 border-0 rounded shadow-sm participante-w";

        const fecha = new Date(r.fechaHora);
        const fechaStr = fecha.toLocaleString();

        div.innerHTML = `
            <div class="d-flex flex-column">
                <span class="fw-bold">${r.mensaje}</span>
                <small class="text-muted">${fechaStr}</small>
            </div>
            <button type="button" class="bg-transparent border-0" onclick="eliminarRecordatorio(${r.idRecordatorio}, event)">
                <i class="bi bi-trash text-danger"></i>
            </button>
        `;

        contenedor.appendChild(div);
    });
}



$("comentarioInput").addEventListener("keypress", (e) => {
    if (e.key === "Enter") {
        enviarComentario();
    }
});
