// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function plus() {
	document.getElementById("cantidad-input").stepUp(1);
}

function minus() {
	document.getElementById("cantidad-input").stepDown(1);
}

function añadirAlCarrito(id) {
	const cantidad = document.getElementById("cantidad-input").value;
	buscar(id).then(resultado => {
		if (!resultado) {
			crear(id, cantidad);
		}
		else {
			editar(id, cantidad);
		}

	});
}

function crear(id, cantidad) {
	var carrito = document.getElementById("cant");
	fetch("/api/carritoapi/", {
		method: "POST",
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include',
		body: JSON.stringify({
			IdUsuario: ':)',
			IdProducto: id,
			cantidad: parseInt(cantidad)
		})
	})
		.then(response => response.json())
		.then(data => {
			carrito.innerHTML = parseInt(carrito.innerHTML) + parseInt(cantidad);
			openAlert("Producto añadido al carrito!");
		})
		.catch(error => {
			console.error("Error al hacer POST:", error);
		});
}

function editar(id, cantidad) {
	var carrito = document.getElementById("cant");
	fetch("/api/carritoapi/" + id, {
		method: "PUT",
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include',
		body: JSON.stringify({
			IdUsuario: ':)',
			IdProducto: id,
			cantidad: parseInt(cantidad)
		})
	})
		.then(response => response.json())
		.then(data => {
			carrito.innerHTML = parseInt(carrito.innerHTML) + parseInt(cantidad);
			openAlert("Carrito actualizado!");
		})
		.catch(error => {
			console.error("Error al hacer PUT:", error);
		});
}

function buscar(id) {
	return fetch("/api/carritoapi/" + id, {
		method: "GET",
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include',
	})
		.then(response => {
			// Verificar si la respuesta fue exitosa
			if (!response.ok) {
				// Si no es 2xx (éxito), manejar el error, como el 404 (Not Found)
				throw new Error('Error: ' + response.status + ' - ' + response.statusText);
			}
			return response.json(); // Si la respuesta es exitosa, procesamos el JSON
		})
		.then(data => {
			console.log('Datos recibidos:', data);
			return true;  // Puedes hacer algo con los datos aquí
		})
		.catch(error => {
			console.error("Error al hacer GET:", error);
			// Si ocurre un error, como un 404, manejarlo aquí
			if (error.message.includes("404")) {
				// O lo que desees hacer en caso de no encontrar el recurso
			}
			return false;  // Retorna false si hay error
		});
}


// Función para abrir el alert con efecto fade
function openAlert(mensaje) {
	var alert = document.getElementById("alert");
	var boton = document.getElementById("btn-comprar");
	document.getElementById("label-alert").innerHTML = mensaje;
	boton.disabled = true;
	boton.innerHTML = "<i class='fa-regular fa-circle-check'></i> Producto añadido al carrito!";
	alert.style.display = "block";  // Asegura que se muestre
	setTimeout(function () {
		alert.classList.add("show"); // Añade la clase 'show' para activar el fade
	}, 10); // Breve retraso para aplicar la transición

	// Cerrar el alert automáticamente después de 3 segundos
	setTimeout(function () {
		closeAlert();
		boton.disabled = false;
		boton.innerHTML = "<i class='fa-solid fa-cart-shopping'></i> Añadir al carrito";
	}, 2500); // 3000 ms = 3 segundos
}

// Función para cerrar el alert
function closeAlert() {
	var alert = document.getElementById("alert");
	alert.classList.remove("show"); // Remueve la clase 'show' para desvanecer
	setTimeout(function () {
		alert.style.display = "none"; // Oculta el alert después del fade
	}, 500); // Tiempo igual a la duración del fade
}

function check() {
	const input = document.getElementById("cantidad-input");
	if (parseInt(input.value) > input.max) {
		input.value = input.max;
	}

	if (parseInt(input.value) < 1) {
		input.value = 1;
	}
}