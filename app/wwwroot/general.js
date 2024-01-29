const removeLoadingScreen = () => {
	window.addEventListener('load', () => {
		setTimeout(() => {
			document.querySelector('.loader-container').remove();

		}, 2000);

	});
};
removeLoadingScreen();

const HTTP_BODY_TEMPLATE = [
	['txtAd', 'NONE'],
	['txtSoyad', 'NONE'],
	['txtEmail', 'NONE'],
	['txtFirma', 'NONE'],
	['cmbLisansIsmi', 'NONE'],
	['cmbLisansTuru', 'NONE'],
	['txtLisansId', 'NONE'],
	['txtOldLisansId', 'NONE'],
	['txtAdet', 'NONE'],
	['dtpLisansBitisTarihi', 'NONE'],
	['dtpAspInvoiceDate', 'NONE'],
	['dtpAspVadeBitisTarihi', 'NONE'],
	['txtYunusVade', 'NONE'],
	['txtYunusPayi', 'NONE'],
	['txtPoNumber', 'NONE'],
	['txtYunusInvoiceNo', 'NONE'],
	['cmbSektor', 'NONE'],
	['txtAspInvoice', 'NONE'],
	['txtAspVadeSuresi', 'NONE'],
	['dtpYurtdisiTalimatTarihi', 'NONE'],
	['txtNot', 'NONE'],
	['cmbYunusOdemeDurumu', 'NONE'],
	['cmbSiEndUser', 'NONE'],
	['cmbFirmaOdemesi', 'NONE'],
	['cmbFinansSistemi', 'NONE'],
];


function returnMatchingColumNames() {
	const inputFields = [
		"txtId",
		"txtAd",
		"txtSoyad",
		"txtEmail",
		"txtFirma",
		"cmbLisansIsmi",
		"cmbLisansTuru",
		"txtLisansId",
		"txtOldLisansId",
		"txtAdet",
		"dtpLisansBitisTarihi",
		"dtpAspInvoiceDate",
		"dtpAspVadeBitisTarihi",
		"txtYunusVade",
		"txtYunusPayi",
		"cmbYunusOdemeDurumu",
		"txtPoNumber",
		"txtYunusInvoiceNo",
		"cmbSektor",
		"cmbSiEndUser",
		"txtAspInvoice",
		"txtAspVadeSuresi",
		"cmbFirmaOdemesi",
		"cmbFinansSistemi",
		"dtpYurtdisiTalimatTarihi",
		"txtNot"
	];
	const outputFields = [
		"ID",
		"NAME",
		"SURNAME",
		"EMAIL",
		"COMPANY",
		"LICENSE_NAME",
		"LICENSE_TYPE",
		"LICENSE_ID",
		"OLD_LICENSE_ID",
		"QUANTITY",
		"LICENSE_EXP_DATE",
		"ASP_INVOICE_DATE",
		"ASP_PAY_DUE_DATE",
		"Yunus_TERM_DAYS",
		"Yunus_PAY_AMOUNT",
		"Yunus_PAY_STATUS",
		"PO_NUM",
		"Yunus_INVO_NUM",
		"SECTOR",
		"SI_END_USER",
		"ASP_INVOICE",
		"ASP_PAY_TERM_DAYS",
		"COMP_MADE_PAY_STATUS",
		"ENT_IN_FINSYS_STATUS",
		"FOREIGN_INST_DATE",
		"NOTES"

	];

	let jsonMatching = {};
	for (let i = 0; i < inputFields.length; i++) {
		jsonMatching[inputFields[i]] = outputFields[i];
	}

	return jsonMatching;
}

const allInputs1 = document.querySelectorAll('input');
const allInputs2 = document.querySelectorAll('textarea');
const allInputs3 = document.querySelectorAll('select');


let globalArrForBothGoruntuleAndFillFirst = [];
//view retrieved JSON data in a good looking table
function btnGoruntule_function() {


	let allInputs = [];

	const inputArrays = [allInputs1, allInputs2, allInputs3];
	inputArrays.forEach((arr) => {
		arr.forEach((input) => {
			allInputs.push(input);
		});
	});


	let thereIsAnInput = false;
	let thereIsAnInputArr = [];
	for (let index = 0; index < allInputs.length; index++) {
		let element = allInputs[index];

		if (element instanceof HTMLSelectElement) {
			if (element.selectedIndex !== -1) { // Check if a valid option is selected
				thereIsAnInput = true;
				thereIsAnInputArr.push([element.id, element.value]);
			}
		} else if (element instanceof HTMLInputElement || element instanceof HTMLTextAreaElement) {
			if (element.value.trim() !== "") { // Check if the input or textarea has a non-empty value
				thereIsAnInput = true;
				thereIsAnInputArr.push([element.id, element.value]);

			}
		}
	}
	const container = document.querySelector('#containerOfResults');
	const table = document.createElement('table');

	// Create table header
	const thead = document.createElement('thead');
	const headerRow = document.createElement('tr');

	if (thereIsAnInput) {
		// There is at least one input element with a value or a select element with a selected option.
		console.log("There is an input or select element with a value.");
		console.log(thereIsAnInputArr); // Inputs and their values as a matrix

		// Create an array to store filtered results
		const filteredResults = [];

		// Loop through each object in SQL_JSON
		SQL_JSON.forEach((obj) => {
			let matchingCriteriaCount = 0; // Counter for matching criteria

			// Loop through each key in the object
			for (const key in obj) {
				let content = obj[key];

				// Check if the content of the key matches any element in thereIsAnInputArr
				thereIsAnInputArr.forEach((el) => {
					const inputFieldName = el[0];
					const exampleText = el[1];

					if (content.toString().toLowerCase().includes(exampleText.toLowerCase())) {
						const columnMatches = returnMatchingColumNames();
						if (columnMatches[inputFieldName] === key) {
							matchingCriteriaCount++; // Increment matching criteria count
						}
					}
				});
			}

			// If all criteria match for this object, add it to the filteredResults array
			if (matchingCriteriaCount === thereIsAnInputArr.length) {
				filteredResults.push(obj);
			}
		});

		console.log(filteredResults);
		globalArrForBothGoruntuleAndFillFirst = filteredResults;
		let filteredResultsWithoutExtraSQLID = filteredResults.map((el) => {
			const {
				SQL_ID,
				...rest
			} = el;
			return rest;
		});


		for (const key in filteredResultsWithoutExtraSQLID[0]) {
			const th = document.createElement('th');
			th.textContent = key;
			headerRow.appendChild(th);
		}

		thead.appendChild(headerRow); // Append the header row to the thead
		table.appendChild(thead); // Append the thead to the table


		const tbody = document.createElement('tbody');

		for (const uniqueKey in filteredResultsWithoutExtraSQLID) {
			if (filteredResultsWithoutExtraSQLID.hasOwnProperty(uniqueKey)) {
				const obj = filteredResultsWithoutExtraSQLID[uniqueKey];

				const row = document.createElement('tr');
				for (const key in obj) {
					if (obj.hasOwnProperty(key)) {
						const cell = document.createElement('td');
						cell.textContent = obj[key];
						row.appendChild(cell);
					}
				}
				tbody.appendChild(row);
			}
		}


		table.appendChild(tbody);

		setTimeout(() => {
			document.querySelector('#containerOfResults table thead').innerHTML = `<tr><th>ID</th><th>NAME</th><th>SURNAME</th><th>EMAIL</th><th>COMPANY</th><th>LICENSE_NAME</th><th>LICENSE_TYPE</th><th>LICENSE_ID</th><th>OLD_LICENSE_ID</th><th>QUANTITY</th><th>LICENSE_EXP_DATE</th><th>ASP_INVOICE_DATE</th><th>ASP_PAY_DUE_DATE</th><th>Yunus_TERM_DAYS</th><th>Yunus_PAY_AMOUNT</th><th>Yunus_PAY_STATUS</th><th>PO_NUM</th><th>Yunus_INVO_NUM</th><th>SECTOR</th><th>SI_END_USER</th><th>ASP_INVOICE</th><th>ASP_PAY_TERM_DAYS</th><th>COMP_MADE_PAY_STATUS</th><th>ENT_IN_FINSYS_STATUS</th><th>FOREIGN_INST_DATE</th><th>NOTES</th></tr>`;

		}, 300);
		document.querySelector('#exports').style.display = "block";

	} else {
		console.log("No input or select element with a value found.");
		let filteredResultsWithoutExtraSQLID = SQL_JSON.map((el) => {
			const {
				SQL_ID,
				...rest
			} = el;
			return rest;
		});
		for (const key in filteredResultsWithoutExtraSQLID[0]) {
			const th = document.createElement('th');
			th.textContent = key;
			headerRow.appendChild(th);
		}

		thead.appendChild(headerRow);
		table.appendChild(thead);

		// Create table body
		const tbody = document.createElement('tbody');
		filteredResultsWithoutExtraSQLID.forEach((obj) => {
			const row = document.createElement('tr');
			for (const key in obj) {
				const cell = document.createElement('td');
				cell.textContent = obj[key];
				row.appendChild(cell);
			}
			tbody.appendChild(row);
		});
		table.appendChild(tbody);

		document.querySelector('#exports').style.display = "block";
		globalArrForBothGoruntuleAndFillFirst = filteredResultsWithoutExtraSQLID;
	}


	container.innerHTML = '';
	container.appendChild(table);
}


function DOMInputFinder() {
	let allInputs = [];

	const inputArrays = [allInputs1, allInputs2, allInputs3];
	inputArrays.forEach((arr) => {
		arr.forEach((input) => {
			allInputs.push(input);
		});
	});


	let thereIsAnInput = false;
	let thereIsAnInputArr = [];
	for (let index = 0; index < allInputs.length; index++) {
		let element = allInputs[index];

		if (element instanceof HTMLSelectElement) {
			if (element.selectedIndex !== -1) { // Check if a valid option is selected
				thereIsAnInput = true;
				thereIsAnInputArr.push([element.id, element.value]);
			}
		} else if (element instanceof HTMLInputElement || element instanceof HTMLTextAreaElement) {
			if (element.value.trim() !== "") { // Check if the input or textarea has a non-empty value
				thereIsAnInput = true;
				thereIsAnInputArr.push([element.id, element.value]);

			}
		}
	}
	return [allInputs, thereIsAnInput, thereIsAnInputArr];
}
async function sendRequest(url, data) {
	try {
		const response = await fetch(url, {
			method: "POST",
			body: JSON.stringify(data),
			headers: {
				"Content-Type": "application/json",
			},
		});

		if (response.ok) {
			const result = await response.json();
			const transformedMessage = JSON.stringify(result).toString().toLowerCase();
			if (result && (transformedMessage.includes('failed') || transformedMessage.includes('error'))) {

				console.error('Error message:', result);
				return "error";
			}
			return response;

		} else {
			throw new Error("Bad response from server...");
		}
	} catch (error) {
		console.error("Error:", error);
		throw error;
	}
}

async function btnEkle_function() {
	//block SQL_ID input
	document.querySelector('#txtId').value = '';
	const finder = DOMInputFinder();

	console.log(finder[0].length);
	console.log(finder[2].length);

	if (finder && finder[0].length - 2 == finder[2].length) {
		try {
			// Send the POST request and await the result
			const actionStatus = await sendRequest("/send?type=add", finder[2]);

			if (actionStatus) {
				notificationBox("Successfully Added Record!", "good");
				setTimeout(() => window.location.reload(), 2000);

			} else {
				notificationBox("FAILED Adding Record!", "bad");
			}
		} catch (error) {
			console.error("Request failed:", error);
			notificationBox("Request failed!", "bad");
		}
	} else {
		notificationBox("There are missing fields!", "bad");
	}


}



async function btnGuncelle_function() {
	// Block SQL_ID input
	document.querySelector('#txtId').value = '';

	// Get input data using DOMInputFinder (assuming it's defined elsewhere)
	const finder = DOMInputFinder();

	// Check if there are inputs to update
	if (finder && finder[2].length > 0) {
		// Clone the HTTP_BODY_TEMPLATE
		let sendToServer = [...HTTP_BODY_TEMPLATE];

		// Iterate through finder[2] and update sendToServer accordingly
		finder[2].forEach((innerArray) => {
			sendToServer.forEach((templateInner) => {
				if (innerArray[0] == templateInner[0]) {
					templateInner[1] = innerArray[1];
				}
			});
		});

		let rowID;

		// Show the custom prompt
		const customPrompt = document.getElementById("customPrompt");
		const rowIDInput = document.getElementById("rowID");
		const confirmButton = document.getElementById("confirmButton");

		customPrompt.style.display = "block";

		// Create a promise to handle the button actions
		const buttonActionPromise = new Promise((resolve) => {
			// Define the confirmButtonClickHandler function
			function confirmButtonClickHandler() {
				rowID = rowIDInput.value;
				customPrompt.style.display = "none";
				resolve();
			}

			// Add an event listener to the confirm button
			confirmButton.addEventListener("click", confirmButtonClickHandler);

			// Define the cancelButtonClickHandler function
			function cancelButtonClickHandler() {
				// Remove the event listener for the confirm button
				confirmButton.removeEventListener("click", confirmButtonClickHandler);

				// Hide the custom prompt
				customPrompt.style.display = "none";

				// Resolve the promise to indicate that the action was canceled
				resolve("canceled");
			}

			// Add an event listener to the cancel button
			const cancelButton = document.querySelector("#cancelButton");
			cancelButton.addEventListener("click", cancelButtonClickHandler);
		});

		// Wait for the buttonActionPromise to resolve
		const result = await buttonActionPromise;

		// Check if the action was canceled
		if (result === "canceled") {
			console.log("Action canceled");
		} else {
			// Continue with your code using the 'rowID' value
			sendToServer.push(["SQL_ID", rowID]);

			try {
				// Send the POST request and await the result
				const actionStatus = await sendRequest("/send?type=update", sendToServer);

				if (actionStatus && actionStatus !== "error") {
					notificationBox("Successfully Updated Record!", "good");
					setTimeout(() => window.location.reload(), 2000);
				} else {
					notificationBox("FAILED Updating Record!", "bad");
				}
			} catch (error) {
				console.error("Request failed:", error);
				notificationBox("Request failed!", "bad");
			}
		}
	} else {
		notificationBox("There are missing fields!", "bad");
	}
}








async function btnSil_function() {
	let sendToServer = [...HTTP_BODY_TEMPLATE];
	let rowID;

	// Show the custom prompt
	const customPrompt = document.getElementById("customPrompt");
	const rowIDInput = document.getElementById("rowID");
	const confirmButton = document.getElementById("confirmButton");
	const cancelButton = document.getElementById("cancelButton"); // Add this line

	customPrompt.style.display = "block";

	// Create a promise to handle the button click
	const buttonActionPromise = new Promise((resolve) => {
		confirmButton.addEventListener("click", () => {
			rowID = rowIDInput.value;
			customPrompt.style.display = "none";
			resolve();
		});

		// Add an event listener to the cancel button (added this part)
		cancelButton.addEventListener("click", () => {
			customPrompt.style.display = "none";
			resolve("canceled");
		});
	});

	// Wait for the buttonActionPromise to resolve
	const result = await buttonActionPromise;

	// Check if the action was canceled
	if (result === "canceled") {
		console.log("Action canceled");
		return; // Exit the function if canceled
	}

	// Continue with your code using the 'rowID' value
	sendToServer.push(["SQL_ID", rowID]);

	try {
		// Send the POST request and await the result
		const actionStatus = await sendRequest("/send?type=delete", sendToServer);

		if (actionStatus && actionStatus !== "error") {
			notificationBox("Successfully Deleted Record!", "good");
			setTimeout(() => window.location.reload(), 2000);
		} else {
			notificationBox("FAILED Deleting Record!", "bad");
		}
	} catch (error) {
		console.error("Request failed:", error);
		notificationBox("Request failed!", "bad");
	}
}





function tableToCSV() {
	const table = document.querySelector('#containerOfResults table');

	// Check if a table element was found
	if (table) {
		// Check if the table has a tbody element
		if (table.querySelector('tbody') && !table.querySelector('tbody h1')) {

			const rows = table.querySelectorAll("tr");
			let csv = [];
			for (let i = 0; i < rows.length; i++) {
				const row = [];
				const cols = rows[i].querySelectorAll("td, th");
				for (let j = 0; j < cols.length; j++) {
					row.push(cols[j].textContent);
				}
				csv.push(row.join(","));
			}

			const csvContent = csv.join("\n");
			const blob = new Blob([csvContent], {
				type: "text/csv"
			});
			const url = window.URL.createObjectURL(blob);
			const a = document.createElement("a");
			a.href = url;
			a.download = "Yunus-License-Tracker-OUTPUT.csv";
			a.style.display = "none";
			document.body.appendChild(a);
			a.click();
			window.URL.revokeObjectURL(url);

			return true;
		} else {
			return false;
		}
	} else {
		return false;
	}
}


function btnCSV_function() {
	actionStatus = tableToCSV();
	if (actionStatus) {
		notificationBox("Successfully Downloaded CSV!", "good");
	} else {
		notificationBox("FAILED Downloading CSV!", "bad");

	}
}

function displayIdeasInDOM(dataArray) {
	if (dataArray.length > 0) {
		while (dataArray.length > 10) {
			dataArray.pop();

		}
		// Split each element of the dataArray by commas
		const dataArrayWithSplit = dataArray.map(idea => idea.split(','));

		// Flatten the array to get a single array of elements
		const flattenedArray = dataArrayWithSplit.flat();

		// Create an array to hold HTML elements for each idea
		const ideaElements = flattenedArray.map(idea => `<p>${idea}</p>`);

		// Join the idea elements into a single string
		const ideasHTML = ideaElements.join(' ');

		// Create a popup with the ideas
		const myPopup = new Popup({
			id: "idea-popup",
			title: `<img id="ideaicon-popup-inner" class="ideaicon" src="https://cdn-icons-png.flaticon.com/512/10782/10782398.png">Ideas:`,
			content: ideasHTML,
			showImmediately: true
		});
	}
}


function showIdeas(ideaType) {
	if (ideaType) {
		if (ideaType == "company") {
			const companyValues = [];
			for (const item of SQL_JSON) {
				companyValues.push(item.COMPANY);

			}
			console.log(companyValues);
			displayIdeasInDOM(companyValues);
		} else if (ideaType == "license") {
			const licenseValues = [];
			for (const item of SQL_JSON) {
				licenseValues.push(item.LICENSE_NAME);


			}
			console.log(licenseValues);

			displayIdeasInDOM(licenseValues);
		} else if (ideaType == "license_type") {
			const license_typeValues = [];
			for (const item of SQL_JSON) {
				license_typeValues.push(item.LICENSE_TYPE);

			}
			console.log(license_typeValues);

			displayIdeasInDOM(license_typeValues);
		} else if (ideaType == "industry") {
			const industryValues = [];
			for (const item of SQL_JSON) {
				industryValues.push(item.SECTOR);

			}
			console.log(industryValues);

			displayIdeasInDOM(industryValues);
		} else {
			alert("Internal Error within DOM, please refresh the page.");
		}
	}
}


function btnIdeasForFields_function(btnID) {
	switch (btnID) {
		case "ideaicon-company":
			showIdeas("company");
			break;
		case "ideaicon-lisans":
			showIdeas("license");
			break;
		case "ideaicon-lisansturu":
			showIdeas("license_type");
			break;
		case "ideaicon-sektor":
			showIdeas("industry");
			break;

	}
}


function btnAnalytics_function() {
	// Check if globalArrForBothGoruntuleAndFillFirst is defined and contains data
	const data = globalArrForBothGoruntuleAndFillFirst.undefined;
	if (typeof globalArrForBothGoruntuleAndFillFirst !== 'undefined' && data) {
		// Use globalArrForBothGoruntuleAndFillFirst if it's defined and not empty
		tableDataJson = globalArrForBothGoruntuleAndFillFirst.undefined;
		console.log('Using globalArrForBothGoruntuleAndFillFirst:', tableDataJson);
	} else {
		// If globalArrForBothGoruntuleAndFillFirst is not defined or empty, use SQL_JSON
		tableDataJson = SQL_JSON;
		console.log('Using SQL_JSON:', tableDataJson);
	}

	// Store the data in localStorage
	localStorage.setItem('data-for-analysis', JSON.stringify(tableDataJson));
	console.log('Data stored in localStorage:', tableDataJson);

	// Open a new window for data analysis
	window.open('/data-analysis/', '_blank');
	console.log('Opening data analysis window.');
}



function notificationBox(msg, type) {
	const images = ['check-mark.png', 'error.png'];
	const box = document.querySelector('#notification');
	box.querySelector('p').innerText = msg;
	box.querySelector('img').src = (type == 'bad' ? images[1] : images[0]);
	box.style.backgroundColor = (type == 'bad' ? "#ff0000da" : "#009c34da");
	box.style.display = "flex";
	setTimeout(() => {
		box.style.display = 'none'
	}, 5000);

}


function btnFill_function() {
	btnGoruntule_function();
	document.querySelector('#containerOfResults').innerHTML = '';
	document.querySelector('#exports').style.display = 'none';

	let dataToFillWith = null;
	if (globalArrForBothGoruntuleAndFillFirst && globalArrForBothGoruntuleAndFillFirst.hasOwnProperty('undefined')) {
		dataToFillWith = globalArrForBothGoruntuleAndFillFirst['undefined']; // Access data using 'undefined' key
	} else {
		dataToFillWith = globalArrForBothGoruntuleAndFillFirst[0];
	}

	if (dataToFillWith) {
		const columnMap = returnMatchingColumNames();
		for (const fieldName in columnMap) {
			if (columnMap.hasOwnProperty(fieldName)) {
				const columnName = columnMap[fieldName];
				const element = document.getElementById(fieldName);

				if (element) {
					if (element.tagName === 'INPUT' || element.tagName === 'TEXTAREA' || element.tagName === 'SELECT') {
						element.value = dataToFillWith[columnName] || '';
					}
				}
			}
		}
	}
}
//clear frontend temporary input values/cells
function btnTemizle_function() {
	const allInputs1 = document.querySelectorAll('input');
	const allInputs2 = document.querySelectorAll('textarea');
	const allInputs3 = document.querySelectorAll('select');
	let allInputs = [];

	const inputArrays = [allInputs1, allInputs2, allInputs3];
	inputArrays.forEach((arr) => {
		arr.forEach((input) => {
			allInputs.push(input);
		});
	});


	for (let index = 0; index < allInputs.length; index++) {
		let element = allInputs[index];
		if (element instanceof HTMLSelectElement) {
			element.selectedIndex = -1; // Clear dropdown selection
		} else if (element instanceof HTMLInputElement) {
			element.value = ""; // Clear input field
		} else if (element instanceof HTMLTextAreaElement) {
			element.value = ""; // Clear textarea
		}
	}
}

//call clean onload
if (window.location.href.includes("panel")) {
	document.addEventListener('DOMContentLoaded', btnTemizle_function);

}
//event listeners to buttons
function addEventListenersToButtons() {
	const buttonIdArray = ['btnGoruntule', 'btnEkle', 'btnGuncelle', 'btnSil', 'btnTemizle', 'btnFill', 'csvexport', 'analyzedata', 'ideaicon'];
	buttonIdArray.forEach((buttonId) => {
		switch (buttonId) {
			case "btnGoruntule":
				document.getElementById(buttonId).addEventListener('click', btnGoruntule_function);
				break;
			case "btnEkle":
				document.getElementById(buttonId).addEventListener('click', btnEkle_function);
				break;
			case "btnGuncelle":
				document.getElementById(buttonId).addEventListener('click', btnGuncelle_function);
				break;
			case "btnSil":
				document.getElementById(buttonId).addEventListener('click', btnSil_function);
				break;
			case "btnTemizle":
				document.getElementById(buttonId).addEventListener('click', btnTemizle_function);
				break;
			case "btnFill":
				document.getElementById(buttonId).addEventListener('click', btnFill_function);
				break;
			case "csvexport":
				document.getElementById(buttonId).addEventListener('click', btnCSV_function);
				break;
			case "analyzedata":
				document.getElementById(buttonId).addEventListener('click', btnAnalytics_function);
				break;
			case "ideaicon":
				const elementsWithClass = document.getElementsByClassName(buttonId);
				Array.from(elementsWithClass).forEach((el) => {
					el.addEventListener('click', (event) => {
						btnIdeasForFields_function(event.target.id)
					});
				});
				break;
		}


	});
}

//verify SQL data if this is panel page
function verifySQLData() {
	if (window.location.href.includes('panel')) {
		if (SQL_JSON) {
			addEventListenersToButtons();
		}
	}
}

document.addEventListener('DOMContentLoaded', verifySQLData);

document.querySelector('#btnGoruntule').addEventListener('click', () => {
	setTimeout(() => {
		// Check if the containerOfResults element exists
		const containerOfResults = document.querySelector('#containerOfResults');

		if (containerOfResults) {
			const tableBody = containerOfResults.querySelector('table tbody');

			// Check if the tableBody element exists
			if (tableBody) {
				// Simulate a delay (adjust the delay time as needed)
				setTimeout(() => {
					if (tableBody.innerHTML === '') {
						tableBody.innerHTML = '<h1 style="font-size:30px !important;color:red;">No Results</h1>';
					}
				}, 100); // Adjust the delay time as needed
			} else {
				console.error("Could not find the 'tbody' element inside the table.");
			}
		} else {
			console.error("Could not find the 'containerOfResults' element.");
		}
	}, 200);
});