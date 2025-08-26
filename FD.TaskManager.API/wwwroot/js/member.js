const memberUrl = "/api/member";

async function addMember(event) {
    event.preventDefault();
    const addMemberDto = {
        userName: document.getElementById("username").value,
        password: document.getElementById("password").value,
        email: document.getElementById("email").value
    };
    try {
        const response = await fetch(`${memberUrl}/addmember`, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(addMemberDto)
        });
        if (response.ok) {
            alert("Sign up complete!");
            window.location.href = "index.html";
        }
        else if (!response.ok) {
            throw new Error(`There was a problem while handling the request: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error: ${error}`);
    }
}