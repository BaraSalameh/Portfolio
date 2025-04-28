'use client';

import { useEffect, useState } from 'react';
import { useParams, usePathname, useRouter } from 'next/navigation';
import { motion } from 'framer-motion';
import {
    Menu, X, Home, Info, LayoutDashboard, Book, Briefcase, Folder, BadgePercent,
    Languages, PenSquare, MessageSquare, Settings, LogOut
} from 'lucide-react';
import { Paragraph } from '../ui/Paragraph';

type SidebarProps = { role?: 'Admin' | 'Owner' | null };

export default function Sidebar({ role }: SidebarProps) {
    const [isCollapsed, setIsCollapsed] = useState(false);
    const pathname = usePathname();
    const { username } = useParams<{ username: string }>();
    const router = useRouter();
    
    // Define nav links with icons
    const navLinks = [
        { href: '/', label: 'Home', icon: Home },
        ...(username && !role
            ? [{ href: `/client/${username}/about`, label: 'About', icon: Info }]
            : username && role === 'Owner'
            ? [
                  { href: `/owner/${username}/dashboard`, label: 'Dashboard', icon: LayoutDashboard },
                  { href: `/owner/${username}/education`, label: 'Education', icon: Book },
                  { href: `/owner/${username}/experience`, label: 'Experience', icon: Briefcase },
                  { href: `/owner/${username}/project`, label: 'Projects', icon: Folder },
                  { href: `/owner/${username}/skill`, label: 'Skills', icon: BadgePercent },
                  { href: `/owner/${username}/language`, label: 'Languages', icon: Languages },
                  { href: `/owner/${username}/blog-post`, label: 'Blog Post', icon: PenSquare },
                  { href: `/owner/${username}/message`, label: 'Messages', icon: MessageSquare },
                  { href: `/owner/${username}/setting`, label: 'Settings', icon: Settings },
                  { href: `/owner/${username}/logout`, label: 'Logout', icon: LogOut },
              ]
            : []),
    ];

    const handleNavigate = (e: React.MouseEvent, url: string) => {
        e.preventDefault()
        router.push(url); // Navigate to another page
    };

    // Handle outside click for mobile
    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth < 640) { // 640px = sm breakpoint in Tailwind
                setIsCollapsed(true);
            } else {
                setIsCollapsed(false);
            }
        };

        handleResize(); // Set on first render
        window.addEventListener('resize', handleResize);

        return () => window.removeEventListener('resize', handleResize);
    }, []);

    return (
        <div className="bg-green-900">
            {/* Sidebar */}
            <motion.aside
                initial={{ x: 0 }}
                animate={{ x: 0 }}
                transition={{ type: 'tween', duration: 0.3 }}
                className={`flex flex-col p-6 duration-100 sticky top-0`}
            >
                {/* Burger Button INSIDE sidebar */}
                <button
                    onClick={() => setIsCollapsed(!isCollapsed)}
                    className="absolute -right-6 top-3 bg-green-900 p-2 rounded-full cursor-pointer duration-300"
                >
                    <Menu  />
                </button>

                {/* Navigation */}
                <nav className="flex flex-col space-y-3 py-5">
                    {navLinks.map(({ href, label, icon: Icon }) => {
                        const isActive = pathname === href;
                        return (
                            <button
                                key={href}
                                onClick={(e) => !isActive && handleNavigate(e, href)}
                               
                                className={`flex items-center gap-3 py-2 cursor-pointer ${
                                    isActive
                                        ? 'text-gray-300 cursor-not-allowed opacity-50'
                                        : 'hover:text-gray-900'
                                }`}
                            >
                                <Icon />
                                {!isCollapsed && <Paragraph size="sm">{label}</Paragraph>}
                            </button>
                        );
                    })}
                </nav>
            </motion.aside>
        </div>
    );
}
