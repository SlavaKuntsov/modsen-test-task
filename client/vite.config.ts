// vite.config.js
import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [react()],
	server: {
		host: '0.0.0.0', // Позволяет доступ извне
		port: 3000, // Порт, который вы хотите использовать
	},
});
