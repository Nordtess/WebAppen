(() => {
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    const noHover = window.matchMedia('(hover: none)').matches;

    if (prefersReducedMotion || noHover) {
        return;
    }

    // Hämta element som representerar cirkeln; avbryt om den saknas
    const circleElement = document.querySelector('.circle');
    if (!circleElement) {
        return;
    }

    // Positionsobjekt för mus och cirkel (flytande värden för mjuk interpolation)
    const mouse = { x: 0, y: 0 };
    const circle = { x: 0, y: 0 };

    // Flagg som visar om pekaren setts minst en gång (används för initial placering)
    let hasPointer = false;

    // Dölj cirkeln initialt tills pekaren rör sig
    circleElement.style.opacity = '0';

    // Uppdatera musposition vid pekarrörelse; använd passive för bättre prestanda
    window.addEventListener(
        'pointermove',
        (e) => {
            mouse.x = e.clientX;
            mouse.y = e.clientY;

            if (!hasPointer) {
                // Vid första detekteringen placera cirkeln direkt utan interpolering
                hasPointer = true;
                circle.x = mouse.x;
                circle.y = mouse.y;
                circleElement.style.transform = `translate(${circle.x}px, ${circle.y}px)`;
                circleElement.style.opacity = '';
            }
        },
        { passive: true }
    );

    // Smoothing-faktor för interpolation (0 < speed <= 1)
    const speed = 0.15;

    // Animeringsloop: interpolera cirkelns position mot musens position och uppdatera transform
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
