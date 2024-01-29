const setLoadingBeforeChecking = () => {
	const div = document.querySelector('#container-data-checker');
	div.innerHTML = "<img style='max-width:500px' src='../YOUR_GIF_HERE.gif'>";
	return new Promise((resolve) => {
		setTimeout(() => {
			resolve(true);
		}, 8000);
	});
};

const checkDataAvailability = async () => {
	const div = document.querySelector('#container-data-checker');
	const dataStorage = localStorage.getItem("data-for-analysis");
	if (dataStorage && div) {
		data = JSON.parse(dataStorage);
		if (Array.isArray(data)) {
			const counted = data.length; //count of licenses in db
			const companyCount = new Set(data.map(entry => entry.COMPANY)).size; //count of distinct companies
			const paidCount = data.filter(entry => entry.COMP_MADE_PAY_STATUS === "Paid").length; //count of paid licenses (PAID TO ASP)
			const pendingCount = data.filter(entry => entry.COMP_MADE_PAY_STATUS !== "Paid").length; // Count of pending payment licenses (PAID TO ASP)

			const today = new Date(); // Get today's date
			const twoMonthsFromNow = new Date(); // Create a date representing 2 months from now
			twoMonthsFromNow.setMonth(today.getMonth() + 2);

			// Filter the data to find entries with expiration dates within 2 months from today
            const upcomingExpirations = data.filter(entry => {
                const expirationDate = new Date(entry.LICENSE_EXP_DATE);
                
                // Calculate the difference in months between today and expirationDate
                const monthsDifference = (expirationDate.getFullYear() - today.getFullYear()) * 12 + (expirationDate.getMonth() - today.getMonth());
            
                // Check if the expiration date is within 2 months from today
                return monthsDifference <= 2 && monthsDifference >= 0;
            });

			// List the companies with upcoming expirations
			const companiesWithUpcomingExpirations = upcomingExpirations.map(entry => entry.COMPANY);



			// Count the occurrences of each 'SI_END_USER' value
			const endUserCounts = {};
			data.forEach(entry => {
				const endUser = entry.SI_END_USER;
				if (endUserCounts[endUser]) {
					endUserCounts[endUser]++;
				} else {
					endUserCounts[endUser] = 1;
				}
			});

			// Convert the counts into an array of objects for easier sorting
			const endUserCountsArray = Object.keys(endUserCounts).map(endUser => ({
				SI_END_USER: endUser,
				count: endUserCounts[endUser],
			}));

			// Sort the array in descending order based on count
			endUserCountsArray.sort((a, b) => b.count - a.count);

			// Get the top 5 most occurring 'SI_END_USER' values
			const top5EndUsers = endUserCountsArray.slice(0, 5);
            //////
            //////
            //////
            // Create an unordered list element
const ul = document.createElement("ul");

// Loop through the 'top5EndUsers' array and create list items for each entry
top5EndUsers.forEach(user => {
  // Create a list item element
  const li = document.createElement("li");

  // Set the text content of the list item with the 'SI_END_USER' value and count
  li.textContent = `NAME: ${user.SI_END_USER} | No.(${user.count})`;

  // Append the list item to the unordered list
  ul.appendChild(li);
});

let companiesString;

            //////
          
			div.innerHTML = `<div id="results-container">
            <h3><i class="fa-solid fa-magnifying-glass-chart"></i> Analysis Results:</h3>
            <div id="lic-count" class="result-item">
                <label><i class="fa-solid fa-calculator"></i> Total License Count:</label>
                <p id="total-license-count">${counted}</p>
            </div>
            <div id="dist-comp" class="result-item">
                <label><i class="fa-solid fa-calculator"></i> Distinct Company Count:</label>
                <p id="distinct-company-count">${companyCount}</p>
            </div>
            <div id="no-of-paid" class="result-item">
                <label><i class="fa-solid fa-calculator"></i> No. of Paid (they paid to ASP):</label>
                <p id="paid-count">${paidCount}</p>
            </div>
            <div id="no-of-pend" class="result-item">
                <label><i class="fa-solid fa-calculator"></i> No. of Pending Payment (pay to ASP):</label>
                <p id="pending-count">${pendingCount}</p>
            </div>
            <div id="in-2-months" class="result-item">
                <label><i class="fa-solid fa-triangle-exclamation"></i> Will Expire in 2 Months:</label>
                <p id="expiring-companies">${companiesString = (companiesWithUpcomingExpirations.join(', ')? companiesWithUpcomingExpirations.join(', '):"None.")}</p>
            </div>
            <div id="top-5-buyers" class="result-item">
                <label><i class="fa-solid fa-award"></i> Top 5 Buyers (by license entry count, not quantity):</label>
                <ul id="top-buyers-list">
                <span id="listContainer"></span>
                                </ul>
            </div>

            <div id="sold-license-types-count-desc" class="result-item">
            <label><i class="fa-solid fa-list-ol"></i> License Sold Count (top performing to least):</label>
            <ul id="licenses-sold-count-ul">
            <span id="licenses-sold-count"></span>
                            </ul>
        </div>
        </div>
         `;



            // Append the unordered list to a container element in your HTML (e.g., a div with id 'listContainer')
const listContainer = document.getElementById("listContainer");
listContainer.appendChild(ul);
  //////

 // logic and list for licenses-sold-count

  const licenseSoldCounts = {};
 data.forEach(entry => {
     const licenseType = entry.LICENSE_TYPE;
     if (licenseSoldCounts[licenseType]) {
         licenseSoldCounts[licenseType]++;
     } else {
         licenseSoldCounts[licenseType] = 1;
     }
 });
 
 // Convert the counts into an array of objects for easier sorting
 const licenseSoldCountsArray = Object.keys(licenseSoldCounts).map(licenseType => ({
     LICENSE_TYPE: licenseType,
     count: licenseSoldCounts[licenseType],
 }));
 
 // Sort the array in descending order based on count
 licenseSoldCountsArray.sort((a, b) => b.count - a.count);
 
 // Get the license sold counts in descending order
 const topLicenseSoldCounts = licenseSoldCountsArray.map(item => `${item.LICENSE_TYPE}: | No.(${item.count})`);
 
 // Create an unordered list element for license sold counts
 const licenseUl = document.createElement("ul");
 
 // Loop through the 'topLicenseSoldCounts' array and create list items for each entry
 topLicenseSoldCounts.forEach(license => {
     // Create a list item element
     const li = document.createElement("li");
 
     // Set the text content of the list item with the license type and count
     li.textContent = license;
 
     // Append the list item to the unordered list
     licenseUl.appendChild(li);
 });
 
 // Append the license sold counts list to the 'licenses-sold-count-ul' element
 const licensesSoldCountContainer = document.getElementById("licenses-sold-count");
 licensesSoldCountContainer.appendChild(licenseUl);




            //////


		} else {
			div.innerHTML = "<img style='max-width:300px' src='../no-data-err.png'>";

		}

	} else {
		div.innerHTML = "<img style='max-width:300px' src='../no-data-err.png'>";
	}
};

document.addEventListener('DOMContentLoaded', async () => {
	await setLoadingBeforeChecking();
	checkDataAvailability();
});