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
        { id: "equipos", mensaje: "El equipo es obligatorio." }
    ];

    campos.forEach(campo => {
        const input = document.getElementById(campo.id);
        if (!input.value.trim()) {
            input.classList.add("is-invalid");
            input.nextElementSibling.textContent = campo.mensaje;
            isValid = false;
        } else {
            input.classList.remove("is-invalid");
            input.classList.add("is-valid");
        }
    });

    return isValid;
}
function mostrarComentarios(comentarios) {
    const contenedor = $("zona-comentarios");
    contenedor.innerHTML = "";

    if (comentarios.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">No hay comentarios aún.</p>`;
        return;
    }

    comentarios.forEach(comentario => {
        const div = document.createElement("div");
        div.className = "mb-2";

        const fecha = new Date(comentario.fechaHora);
        const fechaStr = fecha.toLocaleString();

        div.innerHTML = `
                <p class="mb-0"><strong>Tú:</strong> ${comentario.texto}</p>
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

async function getEquipos() {
    const token = sessionStorage.getItem("Token");
    try {
        const response = await fetch('/Tarea/GetEquipos', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Error al obtener los equipos');
        }

        const equipos = await response.json();
        const select = document.getElementById("equipos");
        select.innerHTML = "<option value=''>Seleccione un equipo</option>";

        equipos.forEach(equipo => {
            const option = document.createElement("option");
            option.value = equipo.idEquipo;
            option.textContent = equipo.nombre;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error al cargar los equipos:', error);
        alert('Error al cargar los equipos. Por favor intente nuevamente.');
    }
}


async function cargarTarea(id) {
    try {
        await getEquipos();

        const response = await fetch(`/Tarea/Details/${id}`);
        const tarea = await response.json();

        setFieldValues({
            tituloTarea: tarea.titulo,
            descripcionTarea: tarea.descripcion,
            fechaLimite: tarea.fechaLimite,
            estadoTarea: tarea.estado,
            prioridadTarea: tarea.prioridad,
            idTarea: tarea.idTarea,
            idUsuario: tarea.idUsuario,
            idEquipo: tarea.idEquipo || ''
        });
        const selectEquipo = document.getElementById("equipos");
        selectEquipo.value = tarea.idEquipo || '';

        recordatoriosSeleccionados = tarea.recordatoriosList || [];
        comentariosSeleccionados = tarea.comentariosList || [];

        mostrarRecordatorios(recordatoriosSeleccionados);
        mostrarComentarios(comentariosSeleccionados);
        mostrarRecordatoriosNotInTarea(id);
        toggleFormulario();
    } catch (error) {
        console.error("Error al cargar la tarea:", error);
        alert("Error al cargar la tarea. Por favor intente nuevamente.");
    }
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
    cargarTodosLosRecordatorios();
    getEquipos();
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
        IdEquipo: getValue("equipos") || null,
        RecordatoriosList: recordatoriosSeleccionados.map(r => ({
            IdRecordatorio: r.idRecordatorio || 0,
            Mensaje: r.mensaje,
            IdTarea: idTarea ? parseInt(idTarea) : 0,
            IdUsuario: parseInt(getValue("idUsuario")),
            FechaHora: r.fechaHora || new Date().toISOString()
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

function mostrarRecordatoriosNotInTarea(id) {
    const token = sessionStorage.getItem("Token");
    fetch(`/Tarea/GetRecordatoriosNotInTarea/${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            const select = document.getElementById("recordatorios");
            select.innerHTML = "<option value=''>Seleccione un recordatorio</option>";
            data.forEach(recordatorio => {
                const option = document.createElement("option");
                option.value = recordatorio.idRecordatorio;
                option.textContent = recordatorio.mensaje;
                select.appendChild(option);
            });
        })
        .catch(error => console.error('Error al cargar los recordatorios:', error));
}

function cargarTodosLosRecordatorios() {
    const token = sessionStorage.getItem("Token");
    fetch(`/Recordatorio/GetAll`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            const select = document.getElementById("recordatorios");
            select.innerHTML = "<option value=''>Seleccione un recordatorio</option>";
            data.forEach(recordatorio => {
                const option = document.createElement("option");
                option.value = recordatorio.idRecordatorio;
                option.textContent = recordatorio.mensaje;
                select.appendChild(option);
            });
        })
        .catch(error => console.error('Error al cargar recordatorios:', error));
}

function mostrarRecordatorios(recordatorios) {
    const contenedor = document.getElementById("listaRecordatorios");
    contenedor.innerHTML = "";

    if (recordatorios.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">Sin recordatorios aún.</p>`;
        return;
    }

    recordatorios.forEach(recordatorio => {
        const div = document.createElement("div");
        div.className = "d-flex align-items-center bg-participante justify-content-between p-2 border-0 rounded shadow-sm recordatorio-w";
        div.innerHTML = `
                <div class="d-flex align-items-center gap-2">
                    <span>${recordatorio.mensaje}</span>
                </div>
                <button type="button" class="bg-transparent border-0" onclick="eliminarRecordatorio('${recordatorio.idRecordatorio}', event)">
                    <i class="bi bi-trash"></i>
                </button>
            `;

        contenedor.appendChild(div);
    });
}

function agregarRecordatorioATarea() {
    const select = document.getElementById("recordatorios");
    const idRecordatorio = parseInt(select.value);
    const mensaje = select.options[select.selectedIndex]?.text;

    if (!idRecordatorio) return;

    if (recordatoriosSeleccionados.some(r => r.idRecordatorio === idRecordatorio)) {
        alert("Este recordatorio ya está en la tarea.");
        return;
    }

    recordatoriosSeleccionados.push({ idRecordatorio, mensaje });
    mostrarRecordatorios(recordatoriosSeleccionados);
    select.remove(select.selectedIndex);
    select.selectedIndex = 0;
}

function eliminarRecordatorio(idRecordatorio, event) {
    event.stopPropagation();
    idRecordatorio = parseInt(idRecordatorio);

    const recordatorioEliminado = recordatoriosSeleccionados.find(r => r.idRecordatorio === idRecordatorio);
    recordatoriosSeleccionados = recordatoriosSeleccionados.filter(r => r.idRecordatorio !== idRecordatorio);

    mostrarRecordatorios(recordatoriosSeleccionados);

    const select = document.getElementById("recordatorios");
    const yaExiste = Array.from(select.options).some(opt => parseInt(opt.value) === idRecordatorio);
    if (!yaExiste && recordatorioEliminado) {
        const nuevaOpcion = document.createElement("option");
        nuevaOpcion.value = recordatorioEliminado.idRecordatorio;
        nuevaOpcion.text = recordatorioEliminado.mensaje;
        select.appendChild(nuevaOpcion);
    }
}

$("comentarioInput").addEventListener("keypress", (e) => {
    if (e.key === "Enter") {
        enviarComentario();
    }
});