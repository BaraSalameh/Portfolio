'use client'

import { useEffect, useRef, useState } from 'react'
import { useParams, usePathname } from 'next/navigation'
import { motion } from 'framer-motion'

type SidebarProps = { role?:  'Admin' | 'Owner' | null };


export default function Sidebar({ role }: SidebarProps) {
    const [isOpen, setIsOpen] = useState(false);
    const pathname = usePathname();
    const sidebarRef = useRef<HTMLDivElement>(null);
    const { username } = useParams<{ username: string }>();
  
    const navLinks = [
        { href: '/', label: 'Home'},
        ...(username && !role
            ? [
                { href: `/client/${username}/about`, label: 'About' }
            ]
            : username && role == 'Admin' 
                ? [
                    
                ]
                : username && role === 'Owner'
                    ? [
                        { href: `/owner/${username}/education`, label: 'Education' },
                        { href: '/experience', label: 'Experience' },
                        { href: '/projects', label: 'Projects' },
                        { href: '/skills', label: 'Skills' },
                        { href: '/language', label: 'Languages' },
                        { href: '/blog', label: 'Blog' },
                        { href: `/owner/${username}/logout`, label: 'Logout' }
                    ]
                    : [])
    ]

  // Handle click outside to close sidebar
    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (sidebarRef.current && !sidebarRef.current.contains(event.target as Node)) {
                setIsOpen(false)
            }
        }

        if (isOpen) {
            document.addEventListener('mousedown', handleClickOutside)
        } else {
            document.removeEventListener('mousedown', handleClickOutside)
        }

        return () => {
            document.removeEventListener('mousedown', handleClickOutside)
        }
    }, [isOpen]);

    return (
        <>
        {/* ☰ Burger Button */}
        <button
            onClick={() => setIsOpen(true)}
            className="fixed top-4 left-4 z-50 bg-green-900 text-white p-4 rounded-md cursor-pointer"
            aria-label="Open sidebar"
        >
            ☰
        </button>

        {/* Sidebar */}
        <motion.aside
            ref={sidebarRef}
            initial={{ x: '-100%' }}
            animate={{ x: isOpen ? 0 : '-100%' }}
            transition={{ type: 'tween', duration: 0.3 }}
            className="fixed top-0 left-0 h-full w-64 bg-green-900 text-white shadow-xl z-50 p-6 rounded-r-xl text-sm/6 text-center sm:text-left font-[family-name:var(--font-geist-mono)]"
            style={{ overflow: 'hidden' }} // no scroll
        >
            {/* ✕ Close Button */}
            <button
                onClick={() => setIsOpen(false)}
                className="text-white text-xl absolute top-4 right-4 cursor-pointer"
                aria-label="Close sidebar"
            >
                ✕
            </button>

            {/* Nav Links */}
            <nav className="flex flex-col space-y-4 mt-12">
                {navLinks.map(({ href, label }) => {
                    const isActive = pathname === href
                    return (
                        <a
                            key={href}
                            href={isActive ? '#' : href}
                            className={`px-4 py-2 rounded-md transition ${
                                isActive
                                    ? 'bg-gray-600  cursor-not-allowed opacity-60'
                                    : 'hover:bg-gray-700 hover:underline'
                            }`}
                            onClick={(e) => isActive && e.preventDefault()}
                        >
                            {label}
                        </a>
                    )
                })}
            </nav>
        </motion.aside>
        </>
    )
}
