'use client';

import { useEffect, useState } from 'react';
import { useParams, usePathname, useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import { Menu } from 'lucide-react';
import { Paragraph } from '../ui/Paragraph';
import ResponsiveIcon from '../ui/ResponsiveIcon';
import { getNavLinks } from '@/lib/utils/appFunctions';
import React from 'react';

type SidebarProps = { role?: 'Admin' | 'Owner' | null };

export const Sidebar = ({ role }: SidebarProps) => {
    const [isCollapsed, setIsCollapsed] = useState(false);
    const pathname = usePathname();
    const { username } = useParams<{ username: string }>();
    const router = useRouter();
    
    const navLinks = getNavLinks(username, role);

    // Handle outside click for mobile
    useEffect(() => {
        const smBreakpoint = 640;
        const handleResize = () => setIsCollapsed(window.innerWidth < smBreakpoint);

        handleResize();
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    return (
            <div>
                <motion.aside
                    initial={{ x: 0 }}
                    animate={{ x: 0 }}
                    transition={{ type: 'tween', duration: 0.3 }}
                    className={`sticky inset-0 flex flex-col justify-center bg-green-900 p-2 duration-100 min-h-screen`}
                >
                    {/* Burger Button INSIDE sidebar */}
                    <button
                        onClick={() => setIsCollapsed(!isCollapsed)}
                        className={`absolute top-6 right-[-1.5rem] bg-inherit px-2 py-1 rounded-md hover:text-gray-900 cursor-pointer duration-300`}
                        aria-label='Toggle sidebar'
                    >
                        <ResponsiveIcon icon={Menu} />
                    </button>
                    
                    {/* Navigation */}
                    <nav className="flex flex-col space-y-5">
                        {navLinks.map(({ href, label, icon: Icon }) => {
                            const isActive = pathname === href;
                            return (
                                <button
                                    key={href}
                                    onClick={() => !isActive && router.push(href)}
                                
                                    className={`flex items-center gap-3  ${
                                        isActive
                                            ? 'text-gray-300 cursor-not-allowed opacity-50'
                                            : 'hover:text-gray-900 cursor-pointer duration-300'
                                    }`}
                                >
                                    <ResponsiveIcon icon={Icon} />
                                    {!isCollapsed && <Paragraph size="sm">{label}</Paragraph>}
                                </button>
                            );
                        })}
                    </nav>
                </motion.aside>
            </div>
    );
}

export default Sidebar;
