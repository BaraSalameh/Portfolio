export const createImage = (url: string): Promise<HTMLImageElement> => 
    new Promise((resolve, reject) => {
        const image = new Image();
        image.addEventListener('load', () => resolve(image));
        image.addEventListener('error', (err) => reject(err));
        image.setAttribute('crossOrigin', 'anonymous'); // Needed for CORS
        image.src = url;
    });