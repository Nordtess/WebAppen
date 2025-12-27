(() => {
    // Visa visuella "ripple" vid klick, men respektera användarinställningar för reducerad rörelse och enheter utan hover.
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const noHover = window.matchMedia('(hover: none)').matches;

    if (prefersReducedMotion || noHover) {
        return;
    }

    // Skapar en ripple-element positionerat i viewport-koordinater och raderar det när animationen är klar.
    function createRipple(x, y) {
        const ripple = document.createElement('span');
        ripple.className = 'cursor-click-ripple';
        ripple.style.left = `${x}px`;
        ripple.style.top = `${y}px`;
        document.body.appendChild(ripple);

        ripple.addEventListener('animationend', () => {
            ripple.remove();
        });
    }

    // Endast primär (vänster) knapp ska trigga ripple.
    document.addEventListener('pointerdown', (e) => {
        if (e.button !== 0) {
            return;
        }

        createRipple(e.clientX, e.clientY);
    }, { passive: true });
})();
