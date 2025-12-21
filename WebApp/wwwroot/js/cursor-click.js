(() => {
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const noHover = window.matchMedia('(hover: none)').matches;

    if (prefersReducedMotion || noHover) {
        return;
    }

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

    document.addEventListener('pointerdown', (e) => {
        if (e.button !== 0) {
            return;
        }

        createRipple(e.clientX, e.clientY);
    }, { passive: true });
})();
