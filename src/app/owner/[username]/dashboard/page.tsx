'use client';

import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { EducationCard } from "@/components/ui/EducationCard";
import { ExperienceCard } from "@/components/ui/ExperienceCard";
import { Paragraph } from "@/components/ui/Paragraph";
import { userQuery } from "@/lib/apis/owner/userQuery";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { useEffect } from "react";

export default function OwnerDashboardPage() {

    const dispatch = useAppDispatch();
    const { user } = useAppSelector(state => state.owner) as any;
    const { lstEducations } = useAppSelector(state => state.education);
    const { lstExperiences } = useAppSelector(state => state.experience);

    useEffect(() => {
        !user && dispatch(userQuery());
    }, []);
    return (
        <>
        <Header itemsX='center'>
            <Paragraph size='xl' className="font-bold">Dashboard</Paragraph>
        </Header>
        <Main paddingX='md'>
            <div className="columns-1 sm:columns-2 lg:columns-3 gap-4 space-y-3">
                <div className="break-inside-avoid ">
                    <EducationCard lstEducations={lstEducations} />
                </div>
                <div className="break-inside-avoid ">
                    <ExperienceCard lstExperiences={lstExperiences} />
                </div>
            </div>
        </Main>
        </>
    );
}
