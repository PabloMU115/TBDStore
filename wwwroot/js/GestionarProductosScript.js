// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var productoSeleccionado;

function actualizarImagen() {
	const input = document.getElementById("input-imagen-url-producto-crear");
	const viewBtn = document.getElementById("view");
	viewBtn.style.visibility = input.value === "" ? "hidden" : "visible";
}

function verImagenCompleta() {
	const src = document.getElementById("input-imagen-url-producto-crear").value.trim();
	const modal = new bootstrap.Modal(document.getElementById('modalImagen'));

	verificarImagen(src, function (esValida) {
		if (esValida && src && src !== window.location.href && !src.endsWith('/')) {
			document.getElementById('imagenModal').src = src;
			modal.show();
		} else {
			alert("La URL ingresada no es una imagen válida.");
		}
	});
}

function verificarImagen(url, callback) {
	const img = new Image();
	img.onload = () => callback(true);
	img.onerror = () => callback(false);
	img.src = url;
}

function actualizarPrecio() {
	const precioInput = document.getElementById("input-precio-producto-crear");
	const errorSpan = document.getElementById("error-precio-producto");
	if (precioInput.value !== "") {
		errorSpan.textContent = ``;
	}
}

function actualizarStock() {
	const precioInput = document.getElementById("input-stock-producto-crear");
	const errorSpan = document.getElementById("error-stock-producto");
	if (precioInput.value !== "") {
		errorSpan.textContent = ``;
	}
}

function actualizarContadorDesc(textarea) {
	document.getElementById("contador_Create").textContent = `${textarea.value.length}/350 caracteres`;
}

function actualizarContadorNombre(textarea) {
	const nombreInput = document.getElementById("input-nombre-producto-crear");
	const errorSpan = document.getElementById("contador_Nombre");
	if (nombreInput.value !== "") {
		errorSpan.className = "mt-1";
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}
	else {
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}

}

function actualizarContadorNombreEditar(textarea) {
	const nombreInput = document.getElementById("input-nombre-producto-editar");
	const errorSpan = document.getElementById("contador_Nombre_editar");
	if (nombreInput.value !== "") {
		errorSpan.className = "mt-1";
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}
	else {
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}

}

function actualizarContadorDescEditar(textarea) {
	document.getElementById("contador_descripcion_editar").textContent = `${textarea.value.length}/350 caracteres`;
}

function span() {
	const categoriaInput = document.getElementById("input-categoria-producto-crear");
	var errorSpan = document.getElementById("error-producto-categoria");
	if (categoriaInput.value !== "-") {
		errorSpan.textContent = "";
	}
	const proveedorInput = document.getElementById("input-proveedor-producto-crear");
	errorSpan = document.getElementById("error-producto-proveedor");
	if (proveedorInput.value !== "-") {
		errorSpan.textContent = "";
	}
}

var dataTable;

$(document).ready(function () {
	cargarTabla();
});

function cargarTabla() {
	dataTable = $('#tabla-productos').DataTable({
		language: {
			decimal: "",
			emptyTable: "No hay productos disponibles en la tabla",
			info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
			infoEmpty: "Mostrando 0 a 0 de 0 entradas",
			infoFiltered: "(filtrado de _MAX_ entradas totales)",
			lengthMenu: "Mostrar _MENU_ entradas",
			loadingRecords: "Cargando...",
			processing: "Procesando...",
			search: "Buscar:",
			zeroRecords: "No se encontraron productos que coincidan.",
			paginate: {
				first: "Primero",
				last: "Último",
				next: "Siguiente",
				previous: "Anterior"
			},
			aria: {
				sortAscending: ": activar para ordenar columna ascendente",
				sortDescending: ": activar para ordenar columna descendente"
			}
		}
	});
}

function crearProducto() {
	const categoriaInput = document.getElementById("input-categoria-producto-crear");
	const proveedorInput = document.getElementById("input-proveedor-producto-crear");
	const nombreInput = document.getElementById("input-nombre-producto-crear");
	const precioInput = document.getElementById("input-precio-producto-crear");
	const stockInput = document.getElementById("input-stock-producto-crear");
	var error = true;
	if (categoriaInput.value === "-") {
		document.getElementById("error-producto-categoria").textContent = "Debe seleccionar una categoría.";
		error = false;
	}
	if (proveedorInput.value === "-") {
		document.getElementById("error-producto-proveedor").textContent = "Debe seleccionar un proveedor.";
		error = false;
	}
	if (nombreInput.value === "") {
		document.getElementById("contador_Nombre").className = "error-text";
		document.getElementById("contador_Nombre").textContent = "Debe ingresar un nombre.";
		error = false;
	}
	if (precioInput.value === "") {
		document.getElementById("error-precio-producto").textContent = "Debe ingresar un precio.";
		error = false;
	}
	if (stockInput.value === "") {
		document.getElementById("error-stock-producto").textContent = "Debe ingresar un Stock.";
		error = false;
	}


	const ranges = [[65, 90], [97, 122]];
	const [min, max] = ranges[Math.floor(Math.random() * ranges.length)];
	const caracter = String.fromCharCode(Math.floor(Math.random() * (max - min + 1)) + min);

	const producto = {
		IdProducto: "P" + (Math.floor(Math.random() * 99) + 10) + "" + (Math.floor(Math.random() * 99) + 10) + caracter,
		NombreProducto: $("#input-nombre-producto-crear").val(),
		Precio: $("#input-precio-producto-crear").val(),
		Descripcion: $("#input-descripcion-producto-crear").val(),
		StockDisponible: $("#input-stock-producto-crear").val(),
		ImagenUrl: $("#input-imagen-url-producto-crear").val(),
		IdProveedor: $("#input-proveedor-producto-crear").val(),
		IdCategoria: $("#input-categoria-producto-crear").val()
	};

	if (error) {
		$.ajax({
			url: '/api/productoapi/',
			type: "POST",
			data: JSON.stringify(producto),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					const rowData = [
						`<span class="id" id="${data.producto.id}">${data.producto.id}</span>`,
						`<a class="nombre" id="${data.producto.nombre}" href="Tienda/Item/${data.producto.idCategoria}-${data.producto.id}-${data.producto.nombre}">${data.producto.nombre}</a>`,
						`<a href="Tienda/Categorias/${data.producto.idCategoria}-${data.producto.categoria}/Page/1/showAll/False?filter=1">${data.producto.categoria}</a>`,
						data.producto.precio,
						data.producto.stock,
						`<button class="btn-editarProducto btn-warning" onclick="openEdit(this)"><i class="fa-solid fa-pen-to-square"></i></button>
							<button class="btn-eliminarProducto btn-danger" onclick="openDelete(this)"><i class="fa-solid fa-trash-can"></i></button>`];

					const rowNode = dataTable.row.add(rowData).draw(false).node();
					$(rowNode).find('td').eq(0).attr('id', `${data.producto.id}`).addClass('id');
					$(rowNode).addClass('tr-tabla');
					console.log(dataTable);
					// Limpiar campos y cerrar modal
					$("#input-nombre-producto-crear").val('');
					$("#input-precio-producto-crear").val(0);
					$("#input-descripcion-producto-crear").val('');
					$("#input-stock-producto-crear").val(0);
					$("#input-imagen-url-producto-crear").val('');
					$("#input-categoria-producto-crear").val('-');
					$("#input-proveedor-producto-crear").val('-');
					$("#ModalCrear").modal("hide");
					alert("Producto creado de forma exitosa");
				}
			}
		});
	}
}

function cerrarCrear() {
	document.getElementById("contador_Nombre").className = "";
	document.getElementById("contador_Nombre").textContent = "0/45 caracteres";
	document.getElementById("error-precio-producto").textContent = "";
	document.getElementById("error-producto-categoria").textContent = "";;
	$("#input-nombre-producto-crear").val('');
	$("#input-precio-producto-crear").val(0);
	$("#input-descripcion-producto-crear").val('');
	$("#input-stock-producto-crear").val(0);
	$("#input-imagen-url-producto-crear").val('');
	$("#input-categoria-producto-crear").val('-');
	$("#input-proveedor-producto-crear").val('-');
	$("#ModalCrear").modal("hide");
}

function openDelete(data) {
	$("#deleteProducto").modal("show");
	let padre = data.closest('.tr-tabla');
	document.getElementById('tagProducto').textContent = '' + padre.querySelector('.nombre').id;
	productoSeleccionado = '' + padre.querySelector('.id').id;
	console.log(productoSeleccionado);
}

function openEdit(data) {
	$("#editProducto").modal("show");
	let padre = data.closest('.tr-tabla');
	productoSeleccionado = '' + padre.querySelector('.id').id;

	let producto = {
		IdProducto: productoSeleccionado
	};

	$.ajax({
		url: '/api/productoapi/' + encodeURIComponent(producto.IdProducto),
		type: "GET",
		success: function (data) {
			if (data.result) {
				console.log(data.producto.idProveedor);
				var name = document.getElementById("input-nombre-producto-editar");
				var desc = document.getElementById("input-descripcion-producto-editar");
				desc.value = data.producto.descripcion;
				name.value = data.producto.nombreProducto;
				document.getElementById("contador_Nombre_editar").textContent = `${name.value.length}/45 caracteres`;
				document.getElementById("contador_descripcion_editar").textContent = `${desc.value.length}/350 caracteres`;
				document.getElementById("input-precio-producto-editar").value = data.producto.precio;
				document.getElementById("input-stock-producto-editar").value = data.producto.stockDisponible;
				document.getElementById("input-imagen-url-producto-editar").value = data.producto.imagenUrl;
				document.getElementById("input-categoria-producto-editar").value = data.producto.idCategoria;
				document.getElementById("input-proveedor-producto-editar").value = data.producto.idProveedor;
				document.getElementById("viewEditar").style.visibility = document.getElementById("input-imagen-url-producto-editar").value === "" ? "hidden" : "visible";
			}
		}
	});
}

function editarProducto() {
	const nombreInput = document.getElementById("input-nombre-producto-editar");
	const precioInput = document.getElementById("input-precio-producto-editar");
	const stockInput = document.getElementById("input-stock-producto-editar");
	const categoriaInput = document.getElementById("input-categoria-producto-editar");
	var error = true;

	if (nombreInput.value === "") {
		document.getElementById("contador_Nombre_editar").className = "error-text";
		document.getElementById("contador_Nombre_editar").textContent = "Debe ingresar un nombre.";
		error = false;
	}
	if (precioInput.value === "") {
		document.getElementById("error-precio-producto-editar").textContent = "Debe ingresar un precio.";
		error = false;
	}
	if (stockInput.value === "") {
		document.getElementById("error-stock-producto-editar").textContent = "Debe ingresar stock disponible.";
		error = false;
	}


	const producto = {
		Id: productoSeleccionado,
		NombreProducto: $("#input-nombre-producto-editar").val(),
		Precio: $("#input-precio-producto-editar").val(),
		Descripcion: $("#input-descripcion-producto-editar").val(),
		StockDisponible: $("#input-stock-producto-editar").val(),
		ImagenUrl: $("#input-imagen-url-producto-editar").val(),
		IdCategoria: $("#input-categoria-producto-editar").val(),
		IdProveedor: $("#input-proveedor-producto-editar").val(),
		NombreCategoria: categoriaInput.options[categoriaInput.selectedIndex].innerText
	};

	if (error) {
		$.ajax({
			url: '/api/productoapi/' + productoSeleccionado,
			type: "PUT",
			data: JSON.stringify(producto),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					alert("Producto editado de forma correcta!");
					let row = dataTable.row(document.getElementById(productoSeleccionado));
					let datos = row.data();
					datos[1] = `<a class="nombre" id="${producto.NombreProducto}" href="Tienda/Item/${producto.IdCategoria}-${producto.Id}-${producto.NombreProducto}">${producto.NombreProducto}</a>`;
					datos[2] = `<a href="Tienda/Categorias/${producto.IdCategoria}-${producto.NombreCategoria}/Page/1/showAll/False?filter=1">${producto.NombreCategoria}</a>`;
					datos[3] = producto.Precio;
					datos[4] = producto.StockDisponible;
					row.data(datos).invalidate().draw();
					productoSeleccionado = '';
					cerrarEditar();
				}
			}
		});
	}
}

function cerrarEditar() {
	document.getElementById("contador_Nombre_editar").className = "";
	document.getElementById("error-precio-producto-editar").textContent = "";
	$("#editProducto").modal("hide");
}

function actualizarPrecioEditar() {
	const precioInput = document.getElementById("input-precio-producto-editar");
	var errorSpan = document.getElementById("error-precio-producto-editar");
	if (precioInput.value !== "") {
		errorSpan.textContent = ``;
	}
	const stockInput = document.getElementById("input-stock-producto-editar");
	errorSpan = document.getElementById("error-stock-producto-editar");
	if (stockInput.value !== "") {
		errorSpan.textContent = ``;
	}
}

function actualizarImagenEditar() {
	const input = document.getElementById("input-imagen-url-producto-editar");
	const viewBtn = document.getElementById("viewEditar");
	viewBtn.style.visibility = input.value === "" ? "hidden" : "visible";
}

function verImagenCompletaEditar() {
	const src = document.getElementById("input-imagen-url-producto-editar").value.trim();
	const modal = new bootstrap.Modal(document.getElementById('modalImagen'));

	verificarImagen(src, function (esValida) {
		if (esValida && src && src !== window.location.href && !src.endsWith('/')) {
			document.getElementById('imagenModal').src = src;
			modal.show();
		} else {
			alert("La URL ingresada no es una imagen válida.");
		}
	});
}

function deleteProducto() {
	const producto = {
		IdProducto: productoSeleccionado
	};

	$.ajax({
		url: '/api/productoapi/',
		type: "DELETE",
		data: JSON.stringify(producto),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (data) {
			if (data.result == 1) {
				alert("Producto eliminado de forma correcta!");
				dataTable.row(document.getElementById(productoSeleccionado)).remove().draw();
				productoSeleccionado = '';
				$("#deleteProducto").modal("hide");
			}
		}
	});
}

window.addEventListener("pageshow", function (event) {
	if (event.persisted || (window.performance && window.performance.navigation.type === 2)) {
		window.location.reload();
	}
});
