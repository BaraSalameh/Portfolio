import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "@/styles/globals.css";
import ReduxProvider from "@/lib/providers/ReduxProvider";

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
});

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
});

export const metadata: Metadata = {
    title: "Portfolio",
    description: "Portfolio app",
};

export default function RootLayout({children}: Readonly<{children: React.ReactNode;}>) {
    return (
        <html lang="en">
            <body className={`${geistSans.variable} ${geistMono.variable} flex flex-col min-h-screen`}>
                <main className="flex-grow">
                    <ReduxProvider>
                        {children}
                    </ReduxProvider>
                </main>
            </body>
        </html>
    );
};