'use client';

import { Main } from "@/components/shared/Main";
import { WidgetCard } from "@/components/ui/widget/WidgetCard";
import { userQuery } from "@/lib/apis/owner/userQuery";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { Briefcase, Clock, GraduationCap, MapPin } from "lucide-react";
import { useCallback, useEffect } from "react";
import EducationForm from "../education/educationForm";
import { deleteEducation } from "@/lib/apis/owner/deleteEducation";
import { educationListQuery } from "@/lib/apis/owner/educationListQuery";
import { sortEducation } from "@/lib/apis/owner/education/sortEducation";
import debounce from "lodash.debounce";
import Loading from "@/components/shared/Loading";
import Profile from "@/components/ui/profile/Profile";

export default function OwnerDashboardPage() {

    const dispatch = useAppDispatch();
    const { loading: userInfoLoading, user } = useAppSelector(state => state.owner);
    const { loading: educationLoading, lstEducations } = useAppSelector(state => state.education);
    const { loading: experienceLoading, lstExperiences } = useAppSelector(state => state.experience);

    const handleDelete = async (id: string) => {
        try {
            await dispatch(deleteEducation(id));
            await dispatch(educationListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    };

    const debouncedSortEducation = useCallback(
        debounce( async (lstIds: string[]) => {
            if (lstIds.length > 0) {
                await dispatch(sortEducation(lstIds));
                await dispatch(educationListQuery());
            }
        
        }, 1000), []
    );

    useEffect(() => {
        !user && dispatch(userQuery());
    }, [user, dispatch]);

    return (
        <>
        <Loading isLoading={userInfoLoading} />
        <Profile
            user={user as any}
        />
        <Main>
            <div className="columns-1 sm:columns-2 lg:columns-3 gap-4 space-y-3 w-full">
                <div className="break-inside-avoid">
                    <WidgetCard
                        isLoading={educationLoading}
                        items={lstEducations}
                        header={{title: 'Education', icon: GraduationCap}}
                        bar={{groupBy: {degree: 'abbreviation'}}}
                        pie={{title:'Degrees Overview', groupBy: {degree: 'abbreviation'}}}
                        list={[
                            {leftKey: {degree: 'abbreviation'}, between: 'at', rightKey: {institution: 'name'}, size:'lg'},
                            {leftKey: {fieldOfStudy: 'name'}, icon: GraduationCap},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                        ]}
                        create={{subTitle: 'Add Education', form: <EducationForm />}}
                        update={{subTitle: 'Update Education', form: <EducationForm />}}
                        del={{subTitle: 'Delete education', message: 'Are you sure?', onDelete: handleDelete }}
                        details={[
                            {leftKey: {degree: 'name'}, between: 'at', rightKey: {institution: 'name'}, size:'lg'},
                            {leftKey: {fieldOfStudy: 'name'}, icon: GraduationCap},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                            {leftKey: 'description', size: 'sm'}
                        ]}
                        onSort={debouncedSortEducation}
                    />
                </div>
            </div>
        </Main>
        </>
    );
}
