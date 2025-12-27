(() => {
    const toasts = document.querySelectorAll('[data-toast]');
    if (!toasts || toasts.length === 0) return;

    toasts.forEach((toast) => {
        const closeBtns = toast.querySelectorAll('[data-toast-close]');

        const dismiss = () => {
            toast.classList.add('toast--out');
            // Låt eventuell CSS-transition köra klart innan elementet tas bort
            window.setTimeout(() => toast.remove(), 140);
        };

        closeBtns.forEach((b) => b.addEventListener('click', dismiss));

        // Dölj automatisk efter 6 sekunder
        window.setTimeout(dismiss, 6000);
    });
})();
