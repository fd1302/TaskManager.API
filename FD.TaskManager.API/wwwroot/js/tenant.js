async function addTenant(event) {
    event.preventDefault();
    const addTenantDto = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        name: document.getElementById("tenantName").value,
        subscription: document.getElementById("subscriptionPlan").value,
        description: document.getElementById("description").value
    }
    try {
        const response = await fetch(`${tenantUrl}/add`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addTenantDto)
        });
        if (response.ok) {
            alert("Signup complete.");
            window.location.href = "index.html";
        }
        else if (!response.ok) {
            throw new Error(`There was an error while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}

async function loadTenants() {
    try {
        const response = await fetch(`${tenantUrl}/gettenants`);
        if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
        const tenants = await response.json();
        const select = document.getElementById("tenantSelect");
        select.innerHTML = `<option value="">-- Select Tenant --</option>`;
        tenants.forEach(tenant => {
            const option = document.createElement("option");
            option.value = tenant.id;
            option.textContent = tenant.name;
            select.appendChild(option);
        });
    } catch (error) {
        console.error("Error loading tenants:", error);
        document.getElementById("tenantSelect").innerHTML =
            "<option value=''>Error loading tenants</option>";
    }
}