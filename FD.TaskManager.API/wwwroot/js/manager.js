const managerUrl = "/api/manager";

async function addManager(event) {
    event.preventDefault();
    const addManagerDto = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        email: document.getElementById("email").value,
        tenantId: document.getElementById("tenantSelect").value
    }
    try {
        const response = await fetch(`${managerUrl}/addmanager`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addManagerDto)
        });
        if (response.ok) {
            alert("Sign up complete!");
            window.location.href = "account.html";
        }
        else if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${await response.json()}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}