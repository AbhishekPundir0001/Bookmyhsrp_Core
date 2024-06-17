// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$.extend(true, $.fn.dataTable.defaults, {
    "pageLength": 30,
    "order": [],
    "responsive": true,
    "lengthChange": true,
    "autoWidth": false
});

var Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});
var startDate;
var endDate;
function showToast(icon, title) {
    Toast.fire({
        icon: icon,
        title: title
    });
}
function destroyDataTableIfExists(selector) {
    if ($.fn.DataTable.isDataTable(document.querySelector(selector))) {
        $(selector).DataTable().clear().destroy();
        $(selector + ' thead').html("");

    }
}
async function postDataFormData(url, formData) {
    NProgress.start();
    if (!url) {
        Toast.fire({ icon: 'error', title: 'URL must be provided' });
        throw new Error('URL must be provided');
    }

    try {
        const response = await fetch(url, {
            method: 'POST',
            body: formData,
            processData: false,
            contentType: false
        });

        // Checking if the response status is different from 2xx and 3xx
        if (!response.ok) {
            Toast.fire({ icon: 'error', title: `HTTP error! Status: ${response.status}` });
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        console.log(response.headers.get("content-type"));

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            Toast.fire({ icon: 'error', title: 'Expected JSON' });
            throw new TypeError("Expected JSON");
        }
        return await response.json();
    } catch (error) {
        console.error('Error:', error);
        Toast.fire({ icon: 'error', title: `${error}` });
        throw error;
    } finally {
        NProgress.done();
    }
}
function bindDynamicSelectHtml(element, data, valueField, textField, defaultOption) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }
    const selectElement = $(element);
    if (!selectElement.length) {
        console.error(`Element with id '${element}' does not exist`);
        return;
    }
    let html = '';
    //add default option
    html += `<option value="">${defaultOption}</option>`;
    data.forEach((item, index) => {
        if (valueField === 'null') {
            html += `<option value="">${item[textField].toUpperCase()}</option>`;
        } else {
            html += `<option value="${item[valueField]}">${item[textField].toUpperCase()}</option>`;
        }

    });
    selectElement.html(html);
    selectElement.select2({
        theme: 'bootstrap-5'
    });
}


function createDataTableHeaders(tableSelector, headers, enableCheckbox = false) {
    $(tableSelector + "thead").html("");
    var headerRow = $('<tr/>');
    $.each(headers, function (i, columnHeader) {


        if (enableCheckbox && columnHeader === "Select") {
            console.log(columnHeader === "Select")
            const checkboxHeader = $('<th/>').html("<input type='checkbox' id='checkboxSelectAll'/>");
            checkboxHeader.append(columnHeader).appendTo(headerRow);
        }

        else {
            $('<th/>').text(columnHeader).appendTo(headerRow);
        }

    });
    $(tableSelector + ' thead').append(headerRow);
}

function createDataTableBody(tableSelector, data, keysToIgnore, createLink, runningNumber = false, enabelCheckbox = false) {

    $(tableSelector + " tbody").html("");
    $.each(data, function (i, item) {
        var row = $('<tr/>');
        if (runningNumber) {
            $('<td/>').text(i + 1).appendTo(row);

        }

        if (enabelCheckbox) {
            $('<td/>').html("<input type='checkbox' id='checkbox' />").appendTo(row)
        }
        $.each(item, function (key, value) {
            if (keysToIgnore && keysToIgnore.includes(key)) {
                $('<td/>').text(value).appendTo(row);
            } else if (createLink) {
                if (value == null) value = '';
                const link = createLink(key, value);
                $('<td/>').html(link).appendTo(row);
            } else {

                if (value == null) value = '';
                $('<td/>').text(value).appendTo(row);
            }
        });
        $(tableSelector + ' tbody').append(row);
    });
}

function addColumnsToDataTable(tableSelector, columns) {
    const headerRow = $(tableSelector + ' thead tr');
    const dataRows = $(tableSelector + ' tbody tr');

    columns.forEach(function (column) {
        const { colName, colOrder, html } = column;
        if (colOrder === -1 || colOrder >= headerRow.children().length) {
            headerRow.append($('<th/>').text(colName));
            dataRows.each(function (i, row) {
                $(row).append($('<td/>').html(html));
            });
        } else if (colOrder === 0) {
            headerRow.prepend($('<th/>').text(colName));
            dataRows.each(function (i, row) {
                $(row).prepend($('<td/>').html(html));
            });
        } else {
            headerRow.children().eq(colOrder - 1).before($('<th/>').text(colName));
            dataRows.each(function (i, row) {
                $(row).children().eq(colOrder - 1).before($('<td/>').html(html));
            });
        }
    });
}

async function fetchAllStates() {
    const url = '/api/v1/state';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForStateInSelect(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}

async function oemVehicleType(data) {
    const url = '/api/v1/oem/by-vehicle-type/' + data;
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForOemVehicleType(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}

async function fetchStates(data) {
    const url = '/api/v1/state';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForStateInSelectById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function sourceLead() {
    const url = '/api/v1/sourceLead';
    try {
        const result = await fetchData(url);

        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForSourceLead(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function sourceLead(data) {
    const url = '/api/v1/sourceLead';
    try {
        const result = await fetchData(url);

        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForSourceLeadById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchMake() {
    const url = '/api/v1/make';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForMakeById(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchMake(data) {
    const url = '/api/v1/make';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForMakeById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function vehicleType() {
    const url = '/api/v1/vehicleType';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForVehicleType(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function vehicleType(data) {
    const url = '/api/v1/vehicleType';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForVehicleTypeById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchFuelType() {
    const url = '/api/v1/fuelType';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForFuelType(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchFuelType(data) {
    const url = '/api/v1/fuelType';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForFuelTypeById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchEngineCondition() {
    const url = '/api/v1/engineCondition';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForEngineCondition(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchEngineCondition(data) {
    const url = '/api/v1/engineCondition';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForEngineConditionById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function productType() {
    const url = '/api/v1/product';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForProductInSelect(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function productType(data) {
    const url = '/api/v1/product';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForProductInSelectById(result, data);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function natureOfBusiness() {
    const url = '/api/v1/natureOfBusiness';
    try {
        const result = await fetchData(url);
        console.log('Data fetched successfully, where the server response data is', result);
        bindHtmlForNatureOfBusiness(result);

    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}
async function fetchData(url) {
    if (!url) {
        Toast.fire({ icon: 'error', title: 'URL must be provided' });
        throw new Error('URL must be provided');
    }
    NProgress.start();
    try {
        const response = await fetch(url);
        // Checking if the response status is different from 2xx and 3xx
        if (!response.ok) {
            Toast.fire({ icon: 'error', title: `HTTP error! Status: ${response.status}` });
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            Toast.fire({ icon: 'error', title: 'Expected JSON' });
            throw new TypeError("Expected JSON");
        }
        NProgress.done();
        return await response.json();
    } catch (error) {
        NProgress.done();
        console.error('Error:', error);
        throw error;
    }
}
function bindHtmlForVehicleType(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-vehicle-type');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Vehicle Type</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForVehicleTypeById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-vehicle-type');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Vehicle Type</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }
        html += `<option value="${item.Id}" ${isSelected ? 'selected' : ''}>${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForMake(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-make');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Make</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.MakerName}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForMakeById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-make');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Make</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }
        html += `<option value="${item.Id}" ${isSelected ? 'selected' : ''}>${item.MakerName}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForFuelType(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-fuel-type');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Fuel type</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForFuelTypeById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-fuel-type');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Fuel type</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }

        html += `<option value="${item.Id}" ${isSelected ? 'selected' : ''}>${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}

function bindHtmlForEngineCondition(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-engine-condition');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Engine Condition</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {

        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}

function bindHtmlForEngineConditionById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-engine-condition');
    if (!selectState.length) {
        console.error("Element with id 'select-make' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Engine Condition</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }
        html += `<option value="${item.Id}" ${isSelected ? 'selected' : ''}>${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}

function bindHtmlForSourceLead(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-source-lead');
    if (!selectState.length) {
        console.error("Element with id 'select-source-lead' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Source Lead</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForSourceLeadById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-source-lead');
    if (!selectState.length) {
        console.error("Element with id 'select-source-lead' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Source Lead</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }
        html += `<option value="${item.Id}"${isSelected ? 'selected' : ''}>${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForStateInSelect(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-state');
    if (!selectState.length) {
        console.error("Element with id 'select-state' does not exist");
        return;
    }
    let html = '';
    //add Select option
    
    html += `<option value="">Select Vehicle Registration State</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        let properCaseStateName = toProperCase(item.HSRPStateName);
        html += `<option value="${item.HSRP_StateID}">${properCaseStateName}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function toProperCase(str) {
    return str.replace(/\w\S*/g, function (txt) {
        return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
    });
}
function bindHtmlForStateInSelectById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-state');
    if (!selectState.length) {
        console.error("Element with id 'select-state' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select State</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.HSRP_StateID.toString()) {
            isSelected = true

        }
        else {
            isSelected = false

        }
        let properCaseStateName = toProperCase(item.HSRPStateName);
       // html += `<option value="${item.HSRP_StateID}">${properCaseStateName}</option>`;
        html += `<option value="${item.HSRP_StateID}" ${isSelected ? 'selected' : ''}>${properCaseStateName}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForOemVehicleType(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-oem-vehicle');
    if (!selectState.length) {
        console.error("Element with id 'select-oem-vehicle' does not exist");
        return;
    }
    let html = '';
    //add Select option

    html += `<option value="">Select Oem Vehicle</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        let properCaseStateName = toProperCase(item.HSRPStateName);
        html += `<option value="${item.HSRP_StateID}">${properCaseStateName}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForProductInSelect(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-product');
    if (!selectState.length) {
        console.error("Element with id 'select-state' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Product</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForProductInSelectById(result, data) {
    if (!Array.isArray(result)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-product');
    if (!selectState.length) {
        console.error("Element with id 'select-product' does not exist");
        return;
    }
    let html = '';
    let isSelected = false;
    //add Select option
    html += `<option value="">Select Product</option>`;
    //html += `<option value="">All</option>`;
    result.forEach((item, index) => {
        if (data === item.Id) {
            isSelected = true;
        }
        else {
            isSelected = false;
        }
        html += `<option value="${item.Id}" ${isSelected ? 'selected' : ''}>${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}
function bindHtmlForNatureOfBusiness(data) {
    if (!Array.isArray(data)) {
        console.error("Invalid data: expecting an array");
        return;
    }

    let selectState = $('#select-nature-of-business');
    if (!selectState.length) {
        console.error("Element with id 'select-nature-of-business' does not exist");
        return;
    }
    let html = '';
    //add Select option
    html += `<option value="">Select Business Nature</option>`;
    //html += `<option value="">All</option>`;
    data.forEach((item, index) => {
        html += `<option value="${item.Id}">${item.Description}</option>`;
    });
    selectState.html(html);
    selectState.select2({
        theme: 'bootstrap-5'
    });
}

function dateRangePicker() {
    var start = moment().subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#reportrange span').html(start.format('DD-MM-YYYY') + ' - ' + end.format('DD-MM-YYYY'));
    }

    $('#reportrange').daterangepicker(
        {
            startDate: start,
            endDate: end,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }
        },
        function (s, e) {
            start = s;
            end = e;
            cb(start, end);
        }
    );

    cb(start, end);

    return function () {
        return {
            startDate: start.format('DD-MM-YYYY'),
            endDate: end.format('DD-MM-YYYY')
        };
    }
}
function datePicker() {
    var start = moment();

    function cb(start, end) {
        $('#reportsingle span').html(start.format('DD-MM-YYYY'));
    }

    $('#reportsingle').daterangepicker(
        {
            startDate: start,
            singleDatePicker: true,

        },
        function (s, e) {
            start = s;
            cb(start);
        }
    );

    cb(start);
    return function () {
        return {
            startDate: start.format('DD-MM-YYYY'),
        };
    }
}
async function postData(url, data) {
    NProgress.start();
    if (!url) {
        Toast.fire({ icon: 'error', title: 'URL must be provided' });
        throw new Error('URL must be provided');
    }

    try {

        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },

            // Converting JSON data to string
            body: JSON.stringify(data)
        });
        // Checking if the response status is different from 2xx and 3xx
        if (!response.ok) {
            const messageBody = await response.json();

            if (response.status === 500) {
                Toast.fire({ icon: 'error', title: `${messageBody.title}` });
                throw new Error(`${messageBody.title}`);
            } else {

                Toast.fire({ icon: 'error', title: `${messageBody.Message}` });
                throw new Error(`${messageBody.Message}`);
            }


        }

        const contentType = response.headers.get("content-type");
        if (!contentType || !contentType.includes("application/json")) {
            Toast.fire({ icon: 'error', title: 'Expected JSON' });
            throw new TypeError("Expected JSON");
        }


        return await response.json();
    } catch (error) {
        console.error('Error:', error);
        Toast.fire({ icon: 'error', title: `${error}` });
        throw error;
    } finally {
        NProgress.done();
    }
}

var resendLink = $("#resendLink");
var resendTimer;


function startResendTimer(durationSeconds) {
    var remainingTime = durationSeconds;
    resendLink.text("Resend OTP in " + remainingTime + "s");
    resendLink.prop("disabled", true);
    resendTimer = setInterval(function () {
        remainingTime--;
        if (remainingTime <= 0) {
            clearInterval(resendTimer);
            resendLink.text("Resend OTP");
            resendLink.prop("disabled", false);
        } else {
            resendLink.text("Resend OTP in " + remainingTime + "s");
        }
    }, 1000);
}


async function downloadReceipt(url, data) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    return await response.blob(); // Assuming the response is a blob
}