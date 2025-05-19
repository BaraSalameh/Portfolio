'use client';

import { Main } from "@/components/shared/Main";
import { WidgetCard } from "@/components/ui/widget/WidgetCard";
import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { Briefcase, Clock, GraduationCap, LocationEdit } from "lucide-react";
import { useCallback, useEffect } from "react";
import EducationForm from "../education/educationForm";
import { sortEducation } from "@/lib/apis/owner/education/sortEducation";
import debounce from "lodash.debounce";
import Loading from "@/components/shared/Loading";
import Profile from "@/components/ui/profile/Profile";
import { ProfileFormData } from "@/lib/schemas/profileSchema";
import { deleteEducation } from "@/lib/apis/owner/education/deleteEducation";
import { educationListQuery } from "@/lib/apis/owner/education/educationListQuery";
import { sortExperience } from "@/lib/apis/owner/experience/sortExperience";
import { experienceListQuery } from "@/lib/apis/owner/experience/experienceListQuery";
import { userFullInfoQuery } from "@/lib/apis/owner/user/userFullInfoQuery";
import ExperienceForm from "../form/experienceForm";
import { deleteExperience } from "@/lib/apis/owner/experience/deleteExperience";

export default function OwnerDashboardPage() {

    const dispatch = useAppDispatch();
    const { loading: userInfoLoading, user } = useAppSelector(state => state.owner);
    const { loading: educationLoading, lstEducations } = useAppSelector(state => state.education);
    const { loading: experienceLoading, lstExperiences } = useAppSelector(state => state.experience);

    const handleEducationDelete = async (id: string) => {
        try {
            await dispatch(deleteEducation(id));
            await dispatch(educationListQuery());
        } catch (err) {
            console.error('Failed to delete:', err);
        }
    };

    const handleExperienceDelete = async (id: string) => {
        try {
            await dispatch(deleteExperience(id));
            await dispatch(experienceListQuery());
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

    const debouncedSortExperience = useCallback(
        debounce( async (lstIds: string[]) => {
            if (lstIds.length > 0) {
                await dispatch(sortExperience(lstIds));
                await dispatch(experienceListQuery());
            }
        
        }, 1000), []
    );

    useEffect(() => {
        !user && dispatch(userFullInfoQuery());
    }, [user, dispatch]);

    return (
        <>
        <Loading isLoading={userInfoLoading} />
        <Profile
            user={user as ProfileFormData}
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
                        del={{subTitle: 'Delete education', message: 'Are you sure?', onDelete: handleEducationDelete }}
                        details={[
                            {leftKey: {degree: 'name'}, between: 'at', rightKey: {institution: 'name'}, size:'lg'},
                            {leftKey: {fieldOfStudy: 'name'}, icon: GraduationCap},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                            {leftKey: 'description', size: 'sm'}
                        ]}
                        onSort={debouncedSortEducation}
                    />
                </div>
                <div className="break-inside-avoid">
                    <WidgetCard
                        isLoading={experienceLoading}
                        items={lstExperiences}
                        header={{title: 'Experience', icon: Briefcase}}
                        bar={{groupBy: 'jobTitle'}}
                        pie={{title:'Experience Overview', groupBy: 'jobTitle'}}
                        list={[
                            {leftKey: 'jobTitle', between: 'at', rightKey: 'companyName', size:'lg'},
                            {leftKey: 'location', icon: LocationEdit},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                        ]}
                        create={{subTitle: 'Add Experience', form: <ExperienceForm />}}
                        update={{subTitle: 'Update Experience', form: <ExperienceForm />}}
                        del={{subTitle: 'Delete Experience', message: 'Are you sure?', onDelete: handleExperienceDelete }}
                        details={[
                            {leftKey: 'jobTitle', between: 'at', rightKey: 'companyName', size:'lg'},
                           {leftKey: 'location', icon: LocationEdit},
                            {leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true},
                            {leftKey: 'description', size: 'sm'}
                        ]}
                        onSort={debouncedSortExperience}
                    />
                </div>
            </div>
        </Main>
        </>
    );
}

