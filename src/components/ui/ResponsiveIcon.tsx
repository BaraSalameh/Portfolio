import React, { useEffect, useState } from 'react';
import { PlusIcon } from 'lucide-react'; // or wherever your icon is from

interface ResponsiveIconProps {
    icon?: React.ElementType;
    className?: string;
    onClick?: () => void;
}

const ResponsiveIcon: React.FC<ResponsiveIconProps> = ({ icon: Icon = PlusIcon, className, onClick }) => {
    const [size, setSize] = useState<number | null>(null);

    useEffect(() => {
        const calculateSize = () => {
            const width = window.innerWidth;
            if (width < 640) return 16;  
            if (width < 768) return 17;  
            if (width < 1024) return 18; 
            if (width < 1280) return 19; 
            return 20;                   
        };

        const handleResize = () => setSize(calculateSize());

        setSize(calculateSize()); // initial
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    if (size === null) return null; // Prevent hydration mismatch

    return <Icon size={size} className={className} onClick={onClick} />;
};

export default ResponsiveIcon;
