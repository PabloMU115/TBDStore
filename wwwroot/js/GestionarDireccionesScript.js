// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let direccionSeleccionada;
function cerrarCrear() {
	document.getElementById("contador_Nombre").className = "";
	document.getElementById("contador_Nombre").textContent = "0/45 caracteres";
	document.getElementById("error-cedula").textContent = "";
	document.getElementById("error-celular").textContent = "";
	document.getElementById("error-nombre").textContent = "";
	const cantonInput = document.getElementById("input-canton");
	cantonInput.innerHTML = '';
	var option = document.createElement("option");
	option.value = "-";
	option.text = "Seleccione su cantón";
	option.selected = true;
	option.hidden = true;
	option.disabled = true;
	cantonInput.appendChild(option);
	const provinciaInput = document.getElementById("input-provincia");
	provinciaInput.value = "-";
	cantonInput.appendChild(option);
	$("#input-nombre-direccion").val('');
	$("#input-cedula-direccion").val('');
	$("#input-celular-direccion").val('');
	$("#input-descripcion-direccion").val('');
	$("#ModalCrear").modal("hide");
}

function actualizarContadorNombre(textarea) {
	const errorSpan = document.getElementById("contador_Nombre");
	document.getElementById("error-nombre").innerText = "";
	if (document.getElementById("input-nombre-direccion").value !== "") {
		errorSpan.className = "mt-1";
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}
	else {
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}

}

function actualizarCampoCelular() {
	document.getElementById("error-celular").innerText = "";
}

function actualizarCampoCedula() {
	document.getElementById("error-cedula").innerText = "";
}

function formatear() {
	input = document.getElementById("input-cedula-direccion");
	cedula = input.value;
	if (cedula.length == 9) {
		let formateado = `${cedula[0]}-${cedula.substring(1, 5)}-${cedula.substring(5)}`;
		input.value = formateado;
	}
}

function actualizarCantones() {
	const provinciaInput = document.getElementById("input-provincia");
	const cantonInput = document.getElementById("input-canton");
	document.getElementById("error-provincia").innerText = "";
	let opciones;
	for (var i = cantonInput.options.length - 1; i > 0; i--) {
		cantonInput.remove(i);
	}
	switch (provinciaInput.value) {
		case "san jose": {
			opciones = [
				{ value: "san jose", text: "San José" },
				{ value: "escazu", text: "Escazú" },
				{ value: "desamparados", text: "Desamparados" },
				{ value: "puriscal", text: "Puriscal" },
				{ value: "tarrazu", text: "Tarrazú" },
				{ value: "aserri", text: "Aserrí" },
				{ value: "moravia", text: "Moravia" },
				{ value: "montes de oca", text: "Montes de Oca" },
				{ value: "turrubares", text: "Turrubares" },
				{ value: "alajuelita", text: "Alajuelita" },
				{ value: "vazquez de coronado", text: "Vázquez de Coronado" },
				{ value: "leon cortes", text: "León Cortés" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "alajuela": {
			opciones = [
				{ value: "alajuela", text: "Alajuela" },
				{ value: "san ramon", text: "San Ramón" },
				{ value: "grecia", text: "Grecia" },
				{ value: "naranjo", text: "Naranjo" },
				{ value: "san carlos", text: "San Carlos" },
				{ value: "zarcero", text: "Zarcero" },
				{ value: "palmares", text: "Palmares" },
				{ value: "poas", text: "Poás" },
				{ value: "atenas", text: "Atenas" },
				{ value: "upala", text: "Upala" },
				{ value: "los chiles", text: "Los Chiles" },
				{ value: "guatuso", text: "Guatuso" },
				{ value: "rio cuarto", text: "Río Cuarto" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "cartago": {
			opciones = [
				{ value: "cartago", text: "Cartago" },
				{ value: "paraiso", text: "Paraíso" },
				{ value: "la union", text: "La Unión" },
				{ value: "jimenez", text: "Jiménez" },
				{ value: "turrialba", text: "Turrialba" },
				{ value: "alvarado", text: "Alvarado" },
				{ value: "oreamuno", text: "Oreamuno" },
				{ value: "el guarco", text: "El Guarco" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "guanacaste": {
			opciones = [
				{ value: "liberia", text: "Liberia" },
				{ value: "nicoya", text: "Nicoya" },
				{ value: "santa cruz", text: "Santa Cruz" },
				{ value: "bagaces", text: "Bagaces" },
				{ value: "canas", text: "Cañas" },
				{ value: "carrillo", text: "Carrillo" },
				{ value: "tilaran", text: "Tilarán" },
				{ value: "la cruz", text: "La Cruz" },
				{ value: "hojancha", text: "Hojancha" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "heredia": {
			opciones = [
				{ value: "heredia", text: "Heredia" },
				{ value: "barva", text: "Barva" },
				{ value: "san rafael", text: "San Rafael" },
				{ value: "santo domingo", text: "Santo Domingo" },
				{ value: "santa barbara", text: "Santa Bárbara" },
				{ value: "san isidro", text: "San Isidro" },
				{ value: "belen", text: "Belén" },
				{ value: "flores", text: "Flores" },
				{ value: "san pablo", text: "San Pablo" },
				{ value: "santa rosa", text: "Santa Rosa" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "limon": {
			opciones = [
				{ value: "limon", text: "Limón" },
				{ value: "puerto limon", text: "Puerto Limón" },
				{ value: "talamanca", text: "Talamanca" },
				{ value: "siquirres", text: "Siquirres" },
				{ value: "matina", text: "Matina" },
				{ value: "guacimo", text: "Guácimo" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
		case "puntarenas": {
			opciones = [
				{ value: "puntarenas", text: "Puntarenas" },
				{ value: "esparza", text: "Esparza" },
				{ value: "montes de oro", text: "Montes de Oro" },
				{ value: "san mateo", text: "San Mateo" },
				{ value: "osa", text: "Osa" },
				{ value: "golfito", text: "Golfito" },
				{ value: "coto brus", text: "Coto Brus" },
				{ value: "quepos", text: "Quepos" },
				{ value: "parrita", text: "Parrita" },
				{ value: "corredores", text: "Corredores" },
				{ value: "buenos aires", text: "Buenos Aires" },
				{ value: "garabito", text: "Garabito" }
			];
			opciones.forEach(function (opcion) {
				var option = document.createElement("option");
				option.value = opcion.value;
				option.text = opcion.text;
				cantonInput.appendChild(option);
			});
		} break;
	}
}

function errorCantones() {
	document.getElementById("error-canton").innerText = "";
}
function actualizarContadorDesc(textarea) {
	document.getElementById("contador_descripcion").textContent = `${textarea.value.length}/300 caracteres`;
}

function seleccionarDeterminada(id) {

	fetch("/api/direccionapi/" + id + "/1", {
		method: "PUT",
		headers: {
			"Content-Type": "application/json"
		},
	})
		.then(response => response.json())
		.then(data => {
			location.reload();
		})
		.catch(error => {
			console.error("Error al hacer DELETE:", error);
		});
}

function eliminar(id) {

	fetch("/api/direccionapi/" + id, {
		method: "DELETE",
		headers: {
			"Content-Type": "application/json"
		}
	})
		.then(response => response.json())
		.then(data => {
			location.reload();
		})
		.catch(error => {
			console.error("Error al hacer DELETE:", error);
		});
}

function crearDireccion() {
	direccion = {
		nombre: document.getElementById("input-nombre-direccion").value,
		numero: document.getElementById("input-celular-direccion").value,
		cedula: document.getElementById("input-cedula-direccion").value,
		provincia: document.getElementById("input-provincia").value,
		canton: document.getElementById("input-canton").value,
		detalles: document.getElementById("input-descripcion-direccion").value
	};
	if (verificacion()) {
		fetch("/api/direccionapi/", {
			method: "POST",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify(direccion)
		})
			.then(response => response.json())
			.then(data => {
				location.reload();
			})
			.catch(error => {
				console.error("Error al hacer PUT:", error);
			});
	}
}

function verificacion() {
	let con = true;
	if (document.getElementById("input-nombre-direccion").value == "") {
		document.getElementById("error-nombre").innerText = "Debe incluir un nombre.";
		con = false;
	}
	if (document.getElementById("input-cedula-direccion").value == "") {
		document.getElementById("error-cedula").innerText = "Debe incluir un número de cédula.";
		con = false
	}
	if (document.getElementById("input-celular-direccion").value == "") {
		document.getElementById("error-celular").innerText = "Debe incluir un número de contacto.";
		con = false
	}
	if (document.getElementById("input-canton").value == "-") {
		document.getElementById("error-canton").innerText = "Debe seleccionar un canton";
		con = false
	}
	if (document.getElementById("input-provincia").value == "-") {
		document.getElementById("error-provincia").innerText = "Debe seleccionar una provincia.";
		con = false
	}
	return con;
}

function formatearGuiones() {
	var input = document.getElementById("input-cedula-direccion");
	if (input.value.includes("-")) {
		input.value = input.value.split("-")[0] + input.value.split("-")[1] + input.value.split("-")[2];
	}
}

function openEditar(id) {
	direccionSeleccionada = id;
	document.getElementById("btn-guardar").style.visibility = "hidden";
	document.getElementById("btn-editar").style.visibility = "visible";
	$("#ModalCrear").modal("show");
	fetch("/api/direccionapi/" + id, {
		method: "GET",
		headers: {
			"Content-Type": "application/json"
		}
	})
		.then(response => response.json())
		.then(data => {
			datos = data.direccion;
			document.getElementById("input-nombre-direccion").value = datos.nombreUsuario;
			document.getElementById("input-cedula-direccion").value = datos.cedulaUsuario;
			document.getElementById("input-celular-direccion").value = datos.numeroUsuario;
			document.getElementById("input-provincia").value = datos.provincia;
			actualizarCantones();
			document.getElementById("input-canton").value = datos.canton;
			document.getElementById("input-descripcion-direccion").value = datos.detallesDireccion;
		})
		.catch(error => {
			console.error("Error al hacer GET:", error);
		});
}

function openCrear() {
	$("#ModalCrear").modal("show");
	document.getElementById("btn-guardar").style.visibility = "visible";
	document.getElementById("btn-editar").style.visibility = "hidden";
	document.getElementById("input-nombre-direccion").value = nombrePredeterminado;
	document.getElementById("input-celular-direccion").value = celularPredeterminado;

}

function editar() {
	direccion = {
		nombre: document.getElementById("input-nombre-direccion").value,
		numero: document.getElementById("input-celular-direccion").value,
		cedula: document.getElementById("input-cedula-direccion").value,
		provincia: document.getElementById("input-provincia").value,
		canton: document.getElementById("input-canton").value,
		detalles: document.getElementById("input-descripcion-direccion").value
	};
	if (verificacion()) {
		fetch("/api/direccionapi/" + direccionSeleccionada, {
			method: "PUT",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify(direccion)
		})
			.then(response => response.json())
			.then(data => {
				location.reload();
			})
			.catch(error => {
				console.error("Error al hacer PUT:", error);
			});
	}
}

//Se cargan los valores predeterminados del usuario cada vez que cree una direccion
//nueva, dandole la opcion de usar esos datos
let nombrePredeterminado;
let celularPredeterminado;
document.addEventListener("DOMContentLoaded", function () {
	nombrePredeterminado = document.getElementById("input-nombre-direccion").value;
	celularPredeterminado = document.getElementById("input-celular-direccion").value;
});