// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var proveedorSeleccionado;

function actualizarContadorNombre(textarea) {
	const errorSpan = document.getElementById("contador_Nombre");
	errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	errorSpan.className = textarea.value ? "mt-1" : "";
}

function actualizarContadorNombreEditar(textarea) {
	const errorSpan = document.getElementById("contador_Nombre_editar");
	errorSpan.textContent = `${textarea.value.length}/45 caracteres`;
	errorSpan.className = textarea.value ? "mt-1" : "";
}

function actualizarContadorDesc(textarea) {
	document.getElementById("contador_Create_Desc").textContent = `${textarea.value.length}/350 caracteres`;
}

function actualizarContadorDir(textarea) {
	document.getElementById("contador_Create_Dir").textContent = `${textarea.value.length}/350 caracteres`;
}

function actualizarContadorDescEditar(textarea) {
	document.getElementById("contador_edit_Desc").textContent = `${textarea.value.length}/350 caracteres`;
}

function actualizarContadorDirEditar(textarea) {
	document.getElementById("contador_edit_Dir").textContent = `${textarea.value.length}/350 caracteres`;
}

var dataTable;

$(document).ready(function () {
	dataTable = $('#tabla-proveedor').DataTable({
		language: {
			decimal: "",
			emptyTable: "No hay proveedores disponibles en la tabla",
			info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
			infoEmpty: "Mostrando 0 a 0 de 0 entradas",
			infoFiltered: "(filtrado de _MAX_ entradas totales)",
			lengthMenu: "Mostrar _MENU_ entradas",
			loadingRecords: "Cargando...",
			processing: "Procesando...",
			search: "Buscar:",
			zeroRecords: "No se encontraron proveedores que coincidan.",
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
});

function crearProveedor() {
	const nombreInput = document.getElementById("input-nombre-proveedor-crear");
	const descripcionInput = document.getElementById("input-descripcion-proveedor-crear");
	const contactoInput = document.getElementById("input-contacto-proveedor-crear");
	const emailInput = document.getElementById("input-email-proveedor-crear");
	const direccionInput = document.getElementById("input-direccion-proveedor-crear");

	const today = new Date();
	const fecha = today.toISOString().split("T")[0];

	let error = true;
	if (nombreInput.value === "") {
		const contador = document.getElementById("contador_Nombre");
		contador.className = "error-text";
		contador.textContent = "Debe ingresar un nombre.";
		error = false;
	}

	const ranges = [[65, 90], [97, 122]];
	const [min, max] = ranges[Math.floor(Math.random() * ranges.length)];
	const caracter = String.fromCharCode(Math.floor(Math.random() * (max - min + 1)) + min);

	const proveedor = {
		IdProveedor: "C" + (Math.floor(Math.random() * 99) + 10) + "" + (Math.floor(Math.random() * 99) + 10) + caracter,
		NombreProveedor: nombreInput.value,
		DescripcionProveedor: descripcionInput.value,
		ContactoProveedor: contactoInput.value,
		EmailProveedor: emailInput.value,
		Direccion: direccionInput.value,
		FechaCreacion: fecha
	};

	if (error) {
		$.ajax({
			url: 'api/ProveedorApi',
			type: "POST",
			data: JSON.stringify(proveedor),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					const rowData = [
						`<span class="id" id="${data.proveedor.idProveedor}">${data.proveedor.idProveedor}</span>`,
						`${data.proveedor.nombreProveedor}`,
						`0`,
						`<button class="btn-editarProveedor btn-warning" onclick="openEdit(this)"><i class="fa-solid fa-pen-to-square"></i></button>
								<button class="btn-eliminarProveedor btn-danger" onclick="openDelete(this)"><i class="fa-solid fa-trash-can"></i></button>`
					];

					const rowNode = dataTable.row.add(rowData).draw(false).node();
					$(rowNode).find('td').eq(0).attr('id', `${data.proveedor.idProveedor}`).addClass('id');
					$(rowNode).find('td').eq(1).attr('id', `${data.proveedor.nombreProveedor}`).addClass('nombre');
					$(rowNode).find('td').eq(2).attr('id', `contador_${data.proveedor.idProveedor}`).addClass('nombre');
					$(rowNode).addClass('tr-tabla');
					cerrarCrear();
					alert("Proveedor creado de forma exitosa");
				}
			}
		});
	}
}

function cerrarCrear() {
	document.getElementById("contador_Nombre").className = "";
	document.getElementById("input-descripcion-proveedor-crear").value = "";
	document.getElementById("input-direccion-proveedor-crear").value = "";
	document.getElementById("input-email-proveedor-crear").value = "";
	document.getElementById("input-contacto-proveedor-crear").value = "";
	document.getElementById("contador_Nombre").textContent = "0/45 caracteres";
	document.getElementById("contador_Create_Desc").textContent = "0/350 caracteres";
	document.getElementById("contador_Create_Dir").textContent = "0/350 caracteres";
	document.getElementById("contador_Nombre").className = "";
	$("#input-nombre-proveedor-crear").val('');
	$("#ModalCrear").modal("hide");
}

function openDelete(data) {
	$("#deleteProveedor").modal("show");
	let padre = data.closest('.tr-tabla');
	document.getElementById('tagProveedor').textContent = padre.querySelector('.nombre').id;
	proveedorSeleccionado = padre.querySelector('.id').id;
}

function openEdit(data) {
	$("#editProveedor").modal("show");
	let padre = data.closest('.tr-tabla');
	proveedorSeleccionado = padre.querySelector('.id').id;

	var name = document.getElementById("input-nombre-proveedor-editar");
	name.value = padre.querySelector('.nombre').id;
	$.ajax({
		url: '/api/proveedorapi/' + proveedorSeleccionado,
		type: "GET",
		success: function (data) {
			if (data.result) {
				document.getElementById("input-nombre-proveedor-editar").value = data.proveedor.nombreProveedor;
				document.getElementById("input-descripcion-proveedor-editar").value = data.proveedor.descripcionProveedor;
				document.getElementById("input-direccion-proveedor-editar").value = data.proveedor.direccion;
				document.getElementById("input-email-proveedor-editar").value = data.proveedor.emailProveedor;
				document.getElementById("input-contacto-proveedor-editar").value = data.proveedor.contactoProveedor;
			}
		}
	});
	document.getElementById("contador_Nombre_editar").textContent = `${name.value.length}/45 caracteres`;
}

function editarProveedor() {
	const nombreInput = document.getElementById("input-nombre-proveedor-editar");
	let error = true;

	if (nombreInput.value === "") {
		const contador = document.getElementById("contador_Nombre_editar");
		contador.className = "error-text";
		contador.textContent = "Debe ingresar un nombre.";
		error = false;
	}

	const proveedor = {
		IdProveedor: proveedorSeleccionado,
		NombreProveedor: $("#input-nombre-proveedor-editar").val(),
		ContactoProveedor: $("#input-contacto-proveedor-editar").val(),
		DescripcionProveedor: $("#input-descripcion-proveedor-editar").val(),
		EmailProveedor: $("#input-email-proveedor-editar").val(),
		Direccion: $("#input-direccion-proveedor-editar").val()
	};

	if (error) {
		$.ajax({
			url: '/api/proveedorapi/' + proveedorSeleccionado,
			type: "PUT",
			data: JSON.stringify(proveedor),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					alert("Proveedor editado de forma correcta!");
					let row = dataTable.row(document.getElementById(proveedorSeleccionado));
					let datos = row.data();
					datos[1] = `${proveedor.NombreProveedor}`;
					row.data(datos).invalidate().draw();
					proveedorSeleccionado = '';
					cerrarEditar();
				}
			}
		});
	}
}

function cerrarEditar() {
	document.getElementById("contador_Nombre_editar").textContent = "0/45 caracteres";
	document.getElementById("contador_Create_Desc").textContent = "0/350 caracteres";
	document.getElementById("contador_Create_Dir").textContent = "0/350 caracteres";
	document.getElementById("contador_Nombre_editar").className = "";
	$("#editProveedor").modal("hide");
}

function deleteProveedor() {
	let contador = document.getElementById("contador_" + proveedorSeleccionado);
	const proveedor = {
		IdProveedor: proveedorSeleccionado
	};

	if (contador.innerText === "0") {
		$.ajax({
			url: '/api/ProveedorApi/',
			type: "DELETE",
			data: JSON.stringify(proveedor),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.result) {
					alert("Proveedor eliminado de forma correcta!");
					dataTable.row(document.getElementById(proveedorSeleccionado)).remove().draw();
					proveedorSeleccionado = '';
					$("#deleteProveedor").modal("hide");
				}
			}
		});
	} else {
		alert("Este proveedor no puede ser eliminado porque no se encuentra vacío.");
	}
}

window.addEventListener("pageshow", function (event) {
	if (event.persisted || (window.performance && window.performance.navigation.type === 2)) {
		window.location.reload();
	}
});