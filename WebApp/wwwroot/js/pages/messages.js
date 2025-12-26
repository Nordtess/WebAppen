document.addEventListener("DOMContentLoaded", () => {
    // Expand/collapse messages
    document.querySelectorAll("[data-message]").forEach((card) => {
        const toggle = card.querySelector("[data-message-toggle]");
        const body = card.querySelector(".message-body");
        if (!(toggle instanceof HTMLButtonElement) || !(body instanceof HTMLElement)) return;

        toggle.addEventListener("click", () => {
            const open = toggle.getAttribute("aria-expanded") === "true";
            toggle.setAttribute("aria-expanded", open ? "false" : "true");
            body.hidden = open;
            card.classList.toggle("is-open", !open);
        });
    });

    // Delete modal
    const modal = document.getElementById("deleteModal");
    const idInput = document.getElementById("deleteIdInput");
    const text = document.getElementById("deleteModalText");

    function setOpen(open) {
        if (!modal) return;
        modal.setAttribute("aria-hidden", open ? "false" : "true");
        document.body.classList.toggle("messages-modal-open", open);

        if (open) {
            const btn = modal.querySelector(".messages-modal__close");
            if (btn instanceof HTMLButtonElement) btn.focus();
        }
    }

    document.addEventListener("click", (e) => {
        const t = e.target;
        if (!(t instanceof HTMLElement)) return;

        const del = t.closest("[data-delete-open]");
        if (del) {
            const id = del.getAttribute("data-delete-id") || "";
            const from = del.getAttribute("data-delete-from") || "";

            if (idInput instanceof HTMLInputElement) idInput.value = id;
            if (text) {
                text.textContent = from ? `Vill du ta bort meddelandet från ${from}?` : "Vill du ta bort meddelandet?";
            }

            setOpen(true);
            return;
        }

        if (t.closest("[data-delete-cancel]")) {
            setOpen(false);
            return;
        }
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && modal?.getAttribute("aria-hidden") === "false") {
            setOpen(false);
        }
    });
});
