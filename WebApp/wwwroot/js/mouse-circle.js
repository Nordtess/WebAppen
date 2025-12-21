(() => {
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const noHover = window.matchMedia('(hover: none)').matches;

    if (prefersReducedMotion || noHover) {
        return;
    }

    const circleElement = document.querySelector('.circle');
    if (!circleElement) {
        return;
    }

    const mouse = { x: 0, y: 0 };
    const circle = { x: 0, y: 0 };

    let hasPointer = false;

    circleElement.style.opacity = '0';

    window.addEventListener(
        'pointermove',
        (e) => {
            mouse.x = e.clientX;
            mouse.y = e.clientY;

            if (!hasPointer) {
                hasPointer = true;
                circle.x = mouse.x;
                circle.y = mouse.y;
                circleElement.style.transform = `translate(${circle.x}px, ${circle.y}px)`;
                circleElement.style.opacity = '';
            }
        },
        { passive: true }
    );

    const speed = 0.15;

    const tick = () => {
        if (hasPointer) {
            circle.x += (mouse.x - circle.x) * speed;
            circle.y += (mouse.y - circle.y) * speed;
            circleElement.style.transform = `translate(${circle.x}px, ${circle.y}px)`;
        }

        window.requestAnimationFrame(tick);
    };

    tick();
})();
