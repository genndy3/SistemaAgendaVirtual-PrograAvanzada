const $ = id => document.getElementById(id);

function toggleFormulario() {
    $("formularioEquipo").classList.toggle("activo");
}

document.addEventListener("click", ({ target }) => {
    const modal = $("formularioEquipo");
    const btnAbrir = $("btnAbrirFormulario");

    if (modal.classList.contains("activo") &&
        !modal.contains(target) &&
        !btnAbrir.contains(target)) {
        modal.classList.remove("activo");
    }
});

document.addEventListener("keydown", ({ key }) => {
    if (key === "Escape") {
        $("formularioEquipo").classList.remove("activo");
    }
});
let participantesSeleccionados = [];

function agregarEquipoModal() {
    setFieldValues({
        nombreEquipo: '',
        descripcionEquipo: '',
        idEquipo: ''
    });

    participantesSeleccionados = [];
    mostrarParticipantes(participantesSeleccionados);

    cargarTodosLosUsuarios();
    toggleFormulario();
}

async function cargarEquipo(id) {
    try {
        const response = await fetch(`/Equipo/Details/${id}`);
        const equipo = await response.json();
        setFieldValues({
            nombreEquipo: equipo.nombre,
            descripcionEquipo: equipo.descripcion,
            idEquipo: equipo.idEquipo
        });
        participantesSeleccionados = equipo.participanteList || [];
        mostrarParticipantes(participantesSeleccionados);
        mostrarParticipantesNotInEquipo(id)
        toggleFormulario();
    } catch (error) {
        console.error("Error al cargar el equipo:", error);
    }
}

function mostrarParticipantesNotInEquipo(id) {
    const token = sessionStorage.getItem("Token");
    fetch(`/Equipo/UsuariosNotInEquipo/${id}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => response.json())
    .then(data => {
        const select = document.getElementById("participantes");
        select.innerHTML = "";
        data.forEach(usuario => {
            const option = document.createElement("option");
            option.value = usuario.idUsuario;
            option.textContent = usuario.nombre;
            select.appendChild(option);
        });
    })
    .catch(error => console.error('Error al cargar los participantes:', error));
}



function mostrarParticipantes(participantes) {

    const contenedor = document.getElementById("listaParticipantes");
    contenedor.innerHTML = "";

    if (participantes.length === 0) {
        contenedor.innerHTML = `<p class="text-muted">Sin participantes aún.</p>`;
        return;
    }

    participantes.forEach(usuario => {
        console.log("usuario:", usuario);
        const div = document.createElement("div");
        div.className = "d-flex align-items-center bg-participante justify-content-between p-2 border-0 rounded shadow-sm participante-w";
        div.innerHTML = `
                    <div class="d-flex align-items-center gap-2">
                        <img src="https://static-00.iconduck.com/assets.00/profile-circle-icon-256x256-cm91gqm2.png" class="rounded-circle" width="35" height="35" alt="Avatar">
                        <span>${usuario.nombre}</span>
                    </div>
                    <button type="button" class="bg-transparent border-0" onclick="eliminarParticipante('${usuario.idUsuario}', event)">
                        <i class="bi bi-trash"></i>
                    </button>
                `;

        contenedor.appendChild(div);
    });
}


function agregarUsuarioAEquipo() {
    const select = document.getElementById("participantes");
    const idUsuario = parseInt(select.value);
    const nombre = select.options[select.selectedIndex]?.text;

    if (!idUsuario) return;

    if (participantesSeleccionados.some(p => p.idUsuario === idUsuario)) {
        alert("Este participante ya está en el equipo.");
        return;
    }

    participantesSeleccionados.push({ idUsuario, nombre });

    mostrarParticipantes(participantesSeleccionados);

    select.remove(select.selectedIndex);

    select.selectedIndex = 0;
}


function eliminarParticipante(idUsuario, event) {
    event.stopPropagation();

    idUsuario = parseInt(idUsuario);
    const participanteEliminado = participantesSeleccionados.find(p => p.idUsuario === idUsuario);

    participantesSeleccionados = participantesSeleccionados.filter(p => p.idUsuario !== idUsuario);
    mostrarParticipantes(participantesSeleccionados);

    const select = document.getElementById("participantes");
    const yaExiste = Array.from(select.options).some(opt => parseInt(opt.value) === idUsuario);
    if (!yaExiste) {
        const nuevaOpcion = document.createElement("option");
        nuevaOpcion.value = participanteEliminado.idUsuario;
        nuevaOpcion.text = participanteEliminado.nombre;
        select.appendChild(nuevaOpcion);
    }
}

function setFieldValues(values) {
    for (const [id, value] of Object.entries(values)) {
        const element = $(id);
        if (element) element.value = value;
    }
}

async function guardarEquipo() {
    const getValue = id => document.getElementById(id)?.value.trim();
    const idEquipo = getValue("idEquipo");

    const nombre = getValue("nombreEquipo");
    if (!nombre) {
        alert("El nombre del equipo es requerido");
        return;
    }

    const equipo = {
        IdEquipo: idEquipo ? parseInt(idEquipo) : 0,
        Nombre: nombre,
        Descripcion: getValue("descripcionEquipo"),
        participanteList: participantesSeleccionados.map(p => ({
            IdUsuario: p.idUsuario,
            Nombre: p.nombre
        }))
    };


    const token = sessionStorage.getItem("Token");
    const url = idEquipo ? `/Equipo/Edit/${idEquipo}` : '/Equipo/Create';
    const method = idEquipo ? 'PUT' : 'POST';

    try {
        const response = await fetch(url, {
            method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(equipo)
        });

        const result = await response.json();

        if (!response.ok) {
            throw new Error(result.message || "Error al guardar el equipo");
        }

        console.log(result.message);
        toggleFormulario();
        location.reload();

    } catch (error) {
        console.error('Error al guardar el equipo:', error);
        alert(error.message || 'Hubo un error al guardar el equipo.');
    }
}

function cargarTodosLosUsuarios() {
    const token = sessionStorage.getItem("Token");
    fetch(`/Equipo/GetUsuarios`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => response.json())
    .then(data => {
        const select = document.getElementById("participantes");
        select.innerHTML = "";
        data.forEach(usuario => {
            const option = document.createElement("option");
            option.value = usuario.idUsuario;
            option.textContent = usuario.nombre;
            select.appendChild(option);
        });
    })
    .catch(error => console.error('Error al cargar usuarios:', error));
}