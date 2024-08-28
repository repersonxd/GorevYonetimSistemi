import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    server: {
        port: 5173, // Sabit bir port numaras� belirleyin
        strictPort: true, // Port kullan�lam�yorsa hata verir
    },
});
