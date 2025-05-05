'use client';

import { Header } from "@/components/shared/Header";
import { Main } from "@/components/shared/Main";
import { Paragraph } from "@/components/ui/Paragraph";
import { WidgetCard } from "@/components/ui/widget/WidgetCard";
import { userQuery } from "@/lib/apis/owner/userQuery";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { Briefcase, Clock, GraduationCap, MapPin } from "lucide-react";
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
                <div className="break-inside-avoid">
                    <WidgetCard
                        items={lstExperiences}
                        header={{title: 'Experience', icon: Briefcase}}
                        bar={{groupBy: 'jobTitle'}}
                        pie={{title:'Job Titles Overview', groupBy: 'jobTitle'}}
                        list={[
                            {leftKey: 'jobTitle', between: 'in', rightKey:'companyName', size: 'lg'},
                            {leftKey: 'location', icon: MapPin},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                        ]}
                    />
                </div>
                <div className="break-inside-avoid">
                    <WidgetCard
                        items={lstEducations}
                        header={{title: 'Education', icon: GraduationCap}}
                        bar={{groupBy: 'degree'}}
                        pie={{title:'Degrees Overview', groupBy: 'degree'}}
                        list={[
                            {leftKey: 'degree', between: 'in', rightKey:'fieldOfStudy', size: 'lg'},
                            {leftKey: 'institution', icon: GraduationCap},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                        ]}
                    />
                </div>
            </div>
        </Main>
        </>
    );
}
