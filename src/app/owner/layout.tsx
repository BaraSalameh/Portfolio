'use client'

import { Container } from "@/components/shared/Container";
import Sidebar from "@/components/shared/Sidebar";

export default function OwnerLayout({children}: Readonly<{children: React.ReactNode;}>) {
    return (
        <div className='flex'>
            <Sidebar role={'Owner'} />
            <Container>
                {children}
            </Container>
        </div>
    );
};