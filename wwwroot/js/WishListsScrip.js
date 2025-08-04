let contenidoReserva = {};

document.addEventListener("DOMContentLoaded", function () {
	const contenedor = document.getElementById("lista-completa");
	const hijos = contenedor.querySelectorAll("div.contenedor-wishlist");

	hijos.forEach(div => {
		contenidoReserva[div.id] = div.innerHTML;
	});
});
function accionesCarrito(id) {
	var div = document.getElementById(id);
	const spinner = document.getElementById('spinner-eliminar-' + id);

	div.style.pointerEvents = "none";
	spinner.style.display = 'flex';
	fetch("/api/carritoapi/" + id, {
		method: "GET",
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include',
	})
		.then(response => {
			// Verificar si la respuesta fue exitosa
			setTimeout(() => {
				if (!response.ok) {
					insertarAlCarrito(id, 1);
				}
				else {
					editarCarrito(id, 1);
				}
				div.style.pointerEvents = "";
			}, 500);
		});
}

function insertarAlCarrito(id, cantidad) {
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
			location.reload();
		})
		.catch(error => {
			console.error("Error al hacer POST:", error);
		});
}

function editarCarrito(id, cantidad) {
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
			location.reload();
		})
		.catch(error => {
			console.error("Error al hacer PUT:", error);
		});
}

function eliminarLista(id) {
	var cantidad = document.getElementById("cantidad");
	var div = document.getElementById(id);
	var nombre = document.getElementById("nombre-" + id).innerText;
	var categoria = document.getElementById("categoria-" + id).innerText;
	const spinner = document.getElementById('spinner-eliminar-' + id);
	var cont = "<h5><a class='nombre-elemento-Carrito' id='" + nombre +
		"' style='text-decoration:none;' href='/Tienda/Item/" + categoria +
		"-" + id + "-" + nombre + "'>" + nombre +
		"</a> ha sido removido de la lista. <a onclick='agregarWishlist(\"" + id + "\")' class='deshacer'>Deshacer</a></h5>";
	div.style.pointerEvents = "none";
	spinner.style.display = 'flex';
	fetch("/api/wishlistapi/" + id, {
		method: "DELETE", 
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include'
	})
		.then(response => {
			if (response.ok) {
				setTimeout(() => {
					div.style.borderRadius = "8px";
					div.style.height = "40px";
					div.style.textAlign = "center";
					div.style.pointerEvents = "";
					div.innerHTML = cont;
				}, 500);
				//setTimeout(() => {
				//	if (parseInt(cantidad.innerHTML) - 1 >= 0) {
				//		cantidad.innerHTML = parseInt(cantidad.innerHTML) - 1;
				//	}
				//	if (cantidad.innerHTML == "0") {
				//		document.getElementById("titulo").innerHTML = "Lista de deseos vacía";
				//	}
				//	div.style.transition = "0.3s ease";
				//	div.style.display = "none";
				//}, 5000);
			}
		})
		.catch(error => {
			console.error("Error al hacer DELETE:", error);
		});
}

function agregarWishlist(id) {
	var div = document.getElementById(id);
	fetch("/api/wishlistapi/", {
		method: "POST",
		headers: {
			"Content-Type": "application/json"
		},
		credentials: 'include',
		body: JSON.stringify({
			IdUsuario: ':)',
			IdProducto: id
		})
	})
		.then(response => {
			div.style.textAlign = "left";
			div.style.height = "110px";
			div.innerHTML = contenidoReserva[id];
		})
		.catch(error => {
			console.error("Error al hacer POST:", error);
		});
}