'use client';

import { motion } from "framer-motion";
import { useState } from "react";
import ResponsiveIcon from "../ui/ResponsiveIcon";
import { ArrowDownUpIcon } from "lucide-react";
import { Paragraph } from "../ui/Paragraph";

export default function Footer() {

    const [isCollapsed, setIsCollapsed] = useState(false);
    
    return (
        <footer className="bg-green-900">
            <motion.aside
                initial={{ x: 0 }}
                animate={{ x: 0 }}
                transition={{ type: 'tween', duration: 0.3 }}
                className={`flex  flex-col p-2 duration-100 sticky bottom-0`}
            >
                <button
                    onClick={() => setIsCollapsed(!isCollapsed)}
                    className="absolute right-10 -top-5.5 bg-green-900 px-2 py-1 rounded-md cursor-pointer duration-300"
                >
                    <ResponsiveIcon icon={ArrowDownUpIcon} />
                </button>
                {isCollapsed && (
                    <div className="flex flex-col sm:flex-row justify-evenly">
                        {/* Quick Links */}
                        <div>
                            <h4 className="font-semibold mb-1">Quick Links</h4>
                            <ul className="space-y-1">
                                <li><a href="/about" className="hover:underline">About</a></li>
                                <li><a href="/projects" className="hover:underline">Projects</a></li>
                                <li><a href="/contact" className="hover:underline">Contact</a></li>
                            </ul>
                        </div>

                        {/* Contact Info */}
                        <div>
                            <h4 className="font-semibold mb-1">Contact</h4>
                            <p>Email: <a href="mailto:your.email@example.com" className="hover:underline">your.email@example.com</a></p>
                            <p>Phone: <a href="tel:+1234567890" className="hover:underline">+1 (234) 567-890</a></p>
                        </div>

                        {/* Social Media */}
                        <div>
                            <h4 className="font-semibold mb-1">Follow Me</h4>
                            <div className="flex space-x-3">
                                {/* Replace these with actual icons or components */}
                                <a href="https://github.com/yourprofile" target="_blank" rel="noopener noreferrer" className="hover:underline">GitHub</a>
                                <a href="https://linkedin.com/in/yourprofile" target="_blank" rel="noopener noreferrer" className="hover:underline">LinkedIn</a>
                                <a href="https://twitter.com/yourprofile" target="_blank" rel="noopener noreferrer" className="hover:underline">Twitter</a>
                            </div>
                        </div>
                    </div>
                )}
                <div className="mx-auto text-center">
                    <Paragraph size='md' >&copy; {new Date().getFullYear()} Portfolio App. All rights reserved.</Paragraph>
                </div>
            </motion.aside>
            
        </footer>
    );
  }
  