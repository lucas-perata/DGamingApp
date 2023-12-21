module.exports = {
  prefix: '',
  purge: {
    content: [
      './src/**/*.{html,ts}',
      './components/**/*.{js,ts,jsx,tsx}'
    ]
  },
  darkMode: 'class', // or 'media' or 'class'
  theme: {
    extend: {
      colors: {
        primary: '#27285C',
        "primary-accent": "#2E2252",
        secondary: '#f5b002',
        "secondary-accent": "#de9f02",
        accent: '#22c55e',
        "accent-focus": "#1fab52",
        text: '#f4f4f4',
        background: '#121212',
      },
      // Custom Daisy UI theme
      daisyui: {
        themes: [
          {
            'my-theme': {
              'primary': 'var(--color-secondary)',
              'primary-focus': 'var(--color-secondary-hover)',
              'primary-content': 'var(--color-text)',
              'accent': 'var(--color-accent)',
              'accent-focus': 'var(--color-accent-hover)',
              'accent-content': 'var(--color-text)',
              'base-100': 'var(--color-background)',
              'base-200': 'var(--color-primary)',
              'base-300': 'var(--color-text)',
              'base-content': 'var(--color-text)',
              'neutral': 'var(--color-text)',
              'neutral-focus': 'var(--color-secondary)',
              'neutral-content': 'var(--color-background)',
              'success': 'var(--color-accent)',
              'success-focus': 'var(--color-accent-hover)',
              'success-content': 'var(--color-text)',
              'info': 'var(--color-primary)',
              'info-focus': 'var(--color-primary-hover)',
              'info-content': 'var(--color-text)',
              'warning': 'var(--color-secondary)',
              'warning-focus': 'var(--color-secondary-hover)',
              'warning-content': 'var(--color-text)',
              'error': 'var(--color-accent)',
              'error-focus': 'var(--color-accent-hover)',
              'error-content': 'var(--color-text)',
            },
          },
        ],
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [require('@tailwindcss/forms'), require('@tailwindcss/typography'), require("daisyui"), require("tw-elements/dist/plugin")],
  daisyui: {
    styled: true,
    themes: true,
    base: true,
    utils: true,
    logs: true,
    rtl: false,
    prefix: "",
    darkTheme: "false",
  },
};
