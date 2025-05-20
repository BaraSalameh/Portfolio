import React, { useEffect, useState } from 'react';
import { PlusIcon } from 'lucide-react'; 

interface ResponsiveIconProps {
    icon?: React.ElementType;
    className?: string;
    onClick?: () => void;
}

const ResponsiveIcon: React.FC<ResponsiveIconProps> = ({ icon: Icon = PlusIcon, className, onClick }) => {
    // const [size, setSize] = useState<number>(21);

    // useEffect(() => {
    //     const calculateSize = () => {
    //         const width = window.innerWidth;
    //         if (width < 640) return 15;  
    //         if (width < 768) return 17;  
    //         if (width < 1024) return 19;
    //         return 30;                   
    //     };

    //     const handleResize = () => setSize(calculateSize());

    //     setSize(calculateSize()); // initial
    //     window.addEventListener('resize', handleResize);
    //     return () => window.removeEventListener('resize', handleResize);
    // }, []);

    return <Icon className={`w-4.5 h-4.5 ${className}`} onClick={onClick} />;
};

export default ResponsiveIcon;
