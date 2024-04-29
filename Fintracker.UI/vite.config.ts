import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from 'tailwindcss'
import mkcert from 'vite-plugin-mkcert'
// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react(), mkcert()],
    server: {
        https: true,
        port: 1337,
        host: 'localhost',
// @ts-ignore
        historyApiFallback: true,
    },
    css: {
        postcss: {
            plugins: [
                tailwindcss,
            ]
        }
    },

})
