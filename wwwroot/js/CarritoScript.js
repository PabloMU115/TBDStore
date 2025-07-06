// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function plus(id) {
	var carrito = document.getElementById("cant");
	var input = document.getElementById(id);
	const spinner = document.getElementById('spinner-' + id);
	var cantidadTotal = document.getElementById("cantidad-items-carrito");
	var suma = 0;
	var botonPlus = document.getElementById("btn-plus-" + id);
	var botonMinus = document.getElementById("btn-minus-" + id);
	if (parseInt(input.value) + 1 <= input.max) {
		input.stepUp(1);
		// Oculta texto del input (sin borrar el valor real)
		input.style.color = 'transparent';

		// Muestra el spinner
		spinner.style.display = 'flex';
		botonMinus.disabled = true;
		botonPlus.disabled = true;
		fetch("/api/carritoapi/" + id, {
			method: "PUT",
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({
				cantidad: 1
			})
		})
			.then(response => response.json())
			.then(data => {
				setTimeout(() => {
					carrito.innerHTML = parseInt(carrito.innerHTML) + 1;
					var unitario = document.getElementById("unitario_" + id).innerHTML.split("₡")[1];
					var cantidad = input.value;
					var total = document.getElementById("total_" + id);
					var subtotal = document.getElementById("precioSubtotal");
					suma = parseInt(cantidadTotal.innerHTML) + 1;
					cantidadTotal.innerHTML = suma + " items";
					precio = parseInt(cantidad) * parseFloat(unitario);
					total.innerHTML = "Precio Total: ₡" + precio;
					subtotal.innerHTML = parseFloat(subtotal.innerHTML) + parseFloat(unitario);
					input.style.color = ''; // restaurar texto
					spinner.style.display = 'none';
					botonMinus.disabled = false;
					botonPlus.disabled = false;
				}, 900);
			})
			.catch(error => {
				console.error("Error al hacer PUT:", error);
			});
	}
	if (parseInt(input.value) > 1) {
		document.getElementById("btn-minus-" + id).innerHTML = "-";
	}
}

function minus(id, nombre, categoria) {
	var input = document.getElementById(id);
	var carrito = document.getElementById("cant");
	const spinner = document.getElementById('spinner-' + id);
	var cantidadTotal = document.getElementById("cantidad-items-carrito");
	var resta = 0;
	var botonPlus = document.getElementById("btn-plus-" + id);
	var botonMinus = document.getElementById("btn-minus-" + id);
	if (parseInt(input.value) - 1 >= input.min) {
		input.stepDown(1);
		// Oculta texto del input (sin borrar el valor real)
		input.style.color = 'transparent';

		// Muestra el spinner
		spinner.style.display = 'flex';
		botonMinus.disabled = true;
		botonPlus.disabled = true;
		if (parseInt(input.value) > input.min) {
			fetch("/api/carritoapi/" + id, {
				method: "PUT",
				headers: {
					"Content-Type": "application/json"
				},
				body: JSON.stringify({
					cantidad: -1
				})
			})
				.then(response => response.json())
				.then(data => {
					setTimeout(() => {
						carrito.innerHTML = parseInt(carrito.innerHTML) - 1;
						var unitario = document.getElementById("unitario_" + id).innerHTML.split("₡")[1];
						var cantidad = input.value;
						var total = document.getElementById("total_" + id);
						var subtotal = document.getElementById("precioSubtotal");
						resta = parseInt(cantidadTotal.innerHTML) - 1;
						cantidadTotal.innerHTML = resta + " items";
						precio = parseInt(cantidad) * parseFloat(unitario);
						total.innerHTML = "Precio Total: ₡" + precio;
						subtotal.innerHTML = parseFloat(subtotal.innerHTML) - parseFloat(unitario);
						input.style.color = ''; // restaurar texto
						spinner.style.display = 'none';
						botonMinus.disabled = false;
						botonPlus.disabled = false;
					}, 900);
				})
				.catch(error => {
					console.error("Error al hacer PUT:", error);
				});
		}
	}
	if (parseInt(input.value) === 1) {
		document.getElementById("btn-minus-" + id).innerHTML = "<i class='fa-solid fa-trash-can'></i>";
	}
	if (parseInt(input.value) === 0) {
		eliminar(id, nombre, categoria);
	}
}

function eliminar(id, nombre, categoria) {
	var carrito = document.getElementById("cant");
	var div = document.getElementById("item-" + id);
	const spinner = document.getElementById('spinner-eliminar-' + id);
	var cantidadIndividual = document.getElementById(id);
	if (parseInt(cantidadIndividual.value) === 0) {
		cantidadIndividual.value = 1;
	}
	var cantidadTotal = document.getElementById("cantidad-items-carrito");
	var resta = 0;

	// deshabilita el div
	div.style.pointerEvents = "none";
	var cont = "<h5><a class='' id='" + nombre + "' href='/Tienda/Item/" + categoria + "-" + id + "-" + nombre + "'>" + nombre + "</a> ha sido removido del carrito.</h5>";

	// Muestra el spinner
	spinner.style.display = 'flex';
	fetch("/api/carritoapi/" + id, {
		method: "DELETE",
		headers: {
			"Content-Type": "application/json"
		},
		body: JSON.stringify({
			cantidad: 1
		})
	})
		.then(response => response.json())
		.then(data => {
			setTimeout(() => {
				var total = document.getElementById("total_" + id);
				var subtotal = document.getElementById("precioSubtotal");
				carrito.innerHTML = parseInt(carrito.innerHTML) - parseInt(cantidadIndividual.value);
				resta = parseInt(cantidadTotal.innerHTML) - parseInt(cantidadIndividual.value);
				cantidadTotal.innerHTML = resta + " items";
				if (resta === 0) {
					document.getElementById("carrito-label").innerHTML = "El carrito se encuentra vacío.";
					document.getElementById("btn-comprar").hidden = true;
				}
				subtotal.innerHTML = parseFloat(subtotal.innerHTML) - parseFloat(total.innerHTML.split("₡")[1]);
				div.style.pointerEvents = "";
				div.innerHTML = cont;
			}, 1000);
		})
		.catch(error => {
			console.error("Error al hacer PUT:", error);
		});
}