/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./app/**/*.{js,jsx,ts,tsx}"],
    presets: [require("nativewind/preset")],
    darkMode: "class",
    theme: {
        extend: {
            fontFamily: {
                sans: ["Nunito", "System"],
            },
            colors: {
                background: "#fefefe",
                foreground: "oklch(0.145 0 0)",

                card: "#ffffff",
                "card-foreground": "oklch(0.145 0 0)",

                popover: "#ffffff",
                "popover-foreground": "oklch(0.145 0 0)",

                primary: "#ff6b35",
                "primary-foreground": "#ffffff",

                secondary: "#f7f3f0",
                "secondary-foreground": "#2d1b1b",

                muted: "#f8f4f1",
                "muted-foreground": "#6b7280",

                accent: "#fef3ee",
                "accent-foreground": "#ea580c",

                destructive: "#ef4444",
                "destructive-foreground": "#ffffff",

                border: "rgba(0,0,0,0.1)",
                input: "#f9f7f4",
                ring: "#ff6b35",

                // food colors
                tomato: "#ff6b47",
                carrot: "#ff8c42",
                lime: "#32d74b",
                blueberry: "#007aff",
                eggplant: "#5856d6",
                banana: "#ffcc02",
                avocado: "#30d158",
                cherry: "#ff2d92",
                orange: "#ff9500",
                grape: "#af52de",
            },
            borderRadius: {
                sm: "0.5rem",
                md: "0.75rem",
                lg: "0.875rem",
                xl: "1rem",
            },
            keyframes: {
                bounceGentle: {
                    "0%, 20%, 50%, 80%, 100%": { transform: "translateY(0)" },
                    "40%": { transform: "translateY(-8px)" },
                    "60%": { transform: "translateY(-4px)" },
                },
                shimmer: {
                    "0%": { backgroundPosition: "-200% 0" },
                    "100%": { backgroundPosition: "200% 0" },
                },
            },
            animation: {
                "bounce-gentle": "bounceGentle 0.6s ease-in-out",
                shimmer: "shimmer 2s infinite",
            },
        },
    },
    plugins: [],
};
