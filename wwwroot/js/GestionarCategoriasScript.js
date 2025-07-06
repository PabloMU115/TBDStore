// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var categoriaSeleccionada;

function actualizarContadorNombre(textarea) {
	const nombreInput = document.getElementById("input-nombre-categoria-crear");
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
	const nombreInput = document.getElementById("input-nombre-categoria-editar");
	const errorSpan = document.getElementById("contador_Nombre_editar");
	if (nombreInput.value !== "") {
		errorSpan.className = "mt-1";
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}
	else {
		errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	}

}

var dataTable;

$(document).ready(function () {
	cargarTabla();
});

function cargarTabla() {
	dataTable = $('#tabla-categoria').DataTable({
		language: {
			decimal: "",
			emptyTable: "No hay categorias disponibles en la tabla",
			info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
			infoEmpty: "Mostrando 0 a 0 de 0 entradas",
			infoFiltered: "(filtrado de _MAX_ entradas totales)",
			lengthMenu: "Mostrar _MENU_ entradas",
			loadingRecords: "Cargando...",
			processing: "Procesando...",
			search: "Buscar:",
			zeroRecords: "No se encontraron categorias que coincidan.",
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

function crearCategoria() {
	const nombreInput = document.getElementById("input-nombre-categoria-crear");
	var error = true;
	if (nombreInput.value === "") {
		document.getElementById("contador_Nombre").className = "error-text";
		document.getElementById("contador_Nombre").textContent = "Debe ingresar un nombre.";
		error = false;
	}

	const ranges = [[65, 90], [97, 122]];
	const [min, max] = ranges[Math.floor(Math.random() * ranges.length)];
	const caracter = String.fromCharCode(Math.floor(Math.random() * (max - min + 1)) + min);

	const categoria = {
		IdCategoria: "C" + (Math.floor(Math.random() * 99) + 10) + "" + (Math.floor(Math.random() * 99) + 10) + caracter,
		NombreCategoria: nombreInput.value
	};

	if (error) {
		$.ajax({
			url: '/api/categoriaapi/',
			type: "POST",
			data: JSON.stringify(categoria),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					const rowData = [
						`<span class="id" id="${data.categoria.idCategoria}">${data.categoria.idCategoria}</span>`,
						`<a class="nombre" id="${data.categoria.nombreCategoria}" href="Tienda/Categorias/${data.categoria.idCategoria}-${categoria.NombreCategoria}/Page/1/showAll/False?filter=1">${data.categoria.nombreCategoria}</a>`,
						`0`,
						`<button class="btn-editarCategoria btn-warning" onclick="openEdit(this)"><i class="fa-solid fa-pen-to-square"></i></button>
							<button class="btn-eliminarCategoria btn-danger" onclick="openDelete(this)"><i class="fa-solid fa-trash-can"></i></button>`
					];

					const rowNode = dataTable.row.add(rowData).draw(false).node();
					$(rowNode).find('td').eq(0).attr('id', `${data.categoria.idCategoria}`).addClass('id');
					$(rowNode).find('td').eq(2).attr('id', `contador_${data.categoria.idCategoria}`);
					$(rowNode).addClass('tr-tabla');
					// Limpiar campos y cerrar modal
					cerrarCrear();
					alert("Categoria creada de forma exitosa");
				}
			}
		});
	}
}

function cerrarCrear() {
	document.getElementById("contador_Nombre").className = "";
	document.getElementById("contador_Nombre").textContent = "0/45 caracteres";
	$("#input-nombre-categoria-crear").val('');
	$("#ModalCrear").modal("hide");
}

function openDelete(data) {
	$("#deleteCategoria").modal("show");
	let padre = data.closest('.tr-tabla');
	document.getElementById('tagCategoria').textContent = '' + padre.querySelector('.nombre').id;
	categoriaSeleccionada = '' + padre.querySelector('.id').id;
	console.log(categoriaSeleccionada);
}

function openEdit(data) {
	$("#editCategoria").modal("show");
	let padre = data.closest('.tr-tabla');
	categoriaSeleccionada = '' + padre.querySelector('.id').id;

	var name = document.getElementById("input-nombre-categoria-editar");
	name.value = '' + padre.querySelector('.nombre').id;;
	document.getElementById("contador_Nombre_editar").textContent = `${name.value.length}/45 caracteres`;
}

function editarCategoria() {
	const nombreInput = document.getElementById("input-nombre-categoria-editar");
	var error = true;
	if (nombreInput.value === "") {
		document.getElementById("contador_Nombre_editar").className = "error-text";
		document.getElementById("contador_Nombre_editar").textContent = "Debe ingresar un nombre.";
		error = false;
	}

	const categoria = {
		IdCategoria: categoriaSeleccionada,
		NombreCategoria: $("#input-nombre-categoria-editar").val()
	};

	if (error) {
		$.ajax({
			url: '/api/categoriaapi/' + categoriaSeleccionada,
			type: "PUT",
			data: JSON.stringify(categoria),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					alert("Categoria editada de forma correcta!");
					let row = dataTable.row(document.getElementById(categoriaSeleccionada));
					let datos = row.data();
					datos[1] = `<a class="nombre" id="${categoria.NombreCategoria}" href="Tienda/Categorias/${categoriaSeleccionada}-${categoria.NombreCategoria}/Page/1/showAll/false">${categoria.NombreCategoria}</a>`;
					row.data(datos).invalidate().draw();
					categoriaSeleccionada = '';
					cerrarEditar();
				}
			}
		});
	}
}

function cerrarEditar() {
	document.getElementById("contador_Nombre_editar").className = "";
	$("#editCategoria").modal("hide");
}

function deleteCategoria() {
	contador = document.getElementById("contador_" + categoriaSeleccionada);
	const categoria = {
		IdCategoria: categoriaSeleccionada
	};

	if (contador.innerText === "0") {
		$.ajax({
			url: '/api/CategoriaApi/',
			type: "DELETE",
			data: JSON.stringify(categoria),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					alert("Categoria eliminada de forma correcta!");
					dataTable.row(document.getElementById(categoriaSeleccionada)).remove().draw();
					categoriaSeleccionada = '';
					$("#deleteCategoria").modal("hide");
				}
			}
		});
	}
	else {
		alert("Esta Categoria no puede ser eliminada por que no se encuentra vacía.");
	}
}

window.addEventListener("pageshow", function (event) {
	if (event.persisted || (window.performance && window.performance.navigation.type === 2)) {
		window.location.reload();
	}
});
