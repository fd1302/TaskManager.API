const modal = document.getElementById("profileModal");
const modalHeader = document.getElementById("modalHeader");
const modalBody = document.getElementById("modalBody");
const modalFooter = document.getElementById("modalFooter");
const closeModal = document.getElementById("closeModal");

function openUpdateModal(id, modalType, entity) {
    if(modalType === "project") {
        projectModalStyle(id, entity);
    } else if(modalType === "board") {
        boardModalStyle(id, entity);
    } else if(modalType === "taskItem") {
        taskItemModalStyle(id, entity);
    } else {
        return "Modal type has wrong value";
    }
    closeModal.onclick = function() { modal.style.display = "none"};
}

function projectModalStyle(id, project) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit project";
    modalHeader.appendChild(title);
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Name: ";
    modalBody.appendChild(nameLabel);
    const nameInput = document.createElement("input");
    nameInput.type = "text";
    nameInput.value = project.name;
    nameInput.id = "projectName";
    modalBody.appendChild(nameInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionInput = document.createElement("input");
    descriptionInput.type = "text";
    descriptionInput.value = project.description;
    descriptionInput.id = "projectDescription";
    modalBody.appendChild(descriptionInput);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateProject(id) };
    modalFooter.appendChild(updateBtn);
}

function boardModalStyle(id, board) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit board";
    modalHeader.appendChild(title);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionInput = document.createElement("input");
    descriptionInput.type = "text";
    descriptionInput.value = board.description;
    descriptionInput.id = "boardDescription";
    modalBody.appendChild(descriptionInput);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateBoard(id) };
    modalFooter.appendChild(updateBtn);
}

function taskItemModalStyle(id, taskItem) {
    modal.style.display = "block";
    modalHeader.innerHTML = "";
    modalBody.innerHTML = "";
    modalFooter.innerHTML = "";
    const title = document.createElement("h2");
    title.textContent = "Edit task";
    modalHeader.appendChild(title);
    const titleLabel = document.createElement("label");
    titleLabel.textContent = "Title: ";
    modalBody.appendChild(titleLabel);
    const titleInput = document.createElement("input");
    titleInput.type = "text";
    titleInput.value = taskItem.title;
    titleInput.id = "taskItemTitle";
    modalBody.appendChild(titleInput);
    const descriptionLabel = document.createElement("label");
    descriptionLabel.textContent = "Description: ";
    modalBody.appendChild(descriptionLabel);
    const descriptionInput = document.createElement("input");
    descriptionInput.type = "text";
    descriptionInput.value = taskItem.description;
    descriptionInput.id = "taskItemDescription";
    modalBody.appendChild(descriptionInput);
    const statusLabel = document.createElement("label");
    statusLabel.textContent = "Status: ";
    modalBody.appendChild(statusLabel);
    const statusSelect = document.createElement("select");
    statusSelect.id = "taskItemStatus";
    modalBody.appendChild(statusSelect);
    const inProgressOption = document.createElement("option");
    inProgressOption.textContent = "In Progress";
    inProgressOption.value = "InProgress";
    statusSelect.appendChild(inProgressOption);
    const completedOption = document.createElement("option");
    completedOption.textContent = "Completed";
    completedOption.value = "Completed";
    statusSelect.appendChild(completedOption);
    const updateBtn = document.createElement("button");
    updateBtn.textContent = "Edit";
    updateBtn.onclick = function() { updateTaskItem(id) };
    modalFooter.appendChild(updateBtn);
}

window.onclick = function(event) {
    if(event.target == modal) {
        modal.style.display = "none";
    }
}