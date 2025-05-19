'use client'

import { Container } from "@/components/shared/Container";
import Sidebar from "@/components/shared/Sidebar";

export default function OwnerLayout({children}: Readonly<{children: React.ReactNode;}>) {
    return (
        <div className='flex'>
            <Sidebar />
            <Container>
                {children}
            </Container>
        </div>
    );
};