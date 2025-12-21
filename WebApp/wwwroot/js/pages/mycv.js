document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector(".cv-toggle");
    if (toggle) {
        toggle.addEventListener("click", async () => {
            const isOn = toggle.classList.toggle("is-on");
            toggle.setAttribute("aria-pressed", isOn ? "true" : "false");

            try {
                const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                const body = new URLSearchParams({ isPrivate: isOn ? "true" : "false" });

                await fetch("/MyCv/SetPrivacy", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded",
                        ...(token ? { "RequestVerificationToken": token } : {})
                    },
                    credentials: "same-origin",
                    body
                });
            } catch {
            }
        });
    }
});
