/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: '#2D7A4A',
        secondary: '#E8A87C',
        accent: '#8BC34A',
        dark: '#1a1a1a',
      }
    },
  },
  plugins: [],
}
