'use client';

import { useAppDispatch, useAppSelector } from "@/lib/store/hooks";
import { Briefcase, Clock, Folder, GraduationCap, LocationEdit, WandSparklesIcon, Link, SearchCodeIcon } from "lucide-react";
import { useEffect } from "react";
import { ProfileFormData } from "@/lib/schemas";
import { useParams } from "next/navigation";
import EducationForm from "../forms/educationForm";
import ExperienceForm from "../forms/experienceForm";
import ProjectTechnologyForm from "../forms/projectTechnologyForm";
import { userByUsernameQuery, userFullInfoQuery } from "@/lib/apis";
import { Loading, WithSkeleton, Main, StaticBackground, ControlledWidget, Profile } from "@/components";
import { useDebouncedSortEducation, useDebouncedSortExperience, useDebouncedSortProject, useHandleEducationDelete, useHandleExperienceDelete, useHandleProjectDelete } from "./handlers";

export default function OwnerDashboardPage() {

    const dispatch = useAppDispatch();
    const { loading: ownerInfoLoading, user: owner } = useAppSelector(state => state.owner);
    const { loading: clientInfoLoading, user: client } = useAppSelector(state => state.client);
    const { loading: projectTechnologyLoading, lstProjectTechnologies } = useAppSelector(state => state.projectTechnology);
    const { loading: educationLoading, lstEducations } = useAppSelector(state => state.education);
    const { loading: experienceLoading, lstExperiences } = useAppSelector(state => state.experience);

    const { role, username } = useParams<{role: 'owner' | 'client' | 'admin', username: string }>();
    const currentUser = {
        user: role === 'owner' ? owner : client,
        isLoading: role === 'owner' ? ownerInfoLoading : clientInfoLoading,
    };

    const handleProjectDelete = useHandleProjectDelete();
    const handleEducationDelete = useHandleEducationDelete();
    const handleExperienceDelete = useHandleExperienceDelete();
    const debouncedSortProject = useDebouncedSortProject();
    const debouncedSortEducation = useDebouncedSortEducation();
    const debouncedSortExperience = useDebouncedSortExperience();

    useEffect(() => {
        if(role === 'owner') {
            !currentUser.user && dispatch(userFullInfoQuery());
        } else if(role === 'client') {
            !currentUser.user && dispatch(userByUsernameQuery(username));
        }
    }, [currentUser.user, dispatch]);
    
    return (
        <>
        <Loading isLoading={!currentUser.user || currentUser.isLoading} />
        <Profile user={currentUser.user as ProfileFormData} />
        <WithSkeleton isLoading={!currentUser.user || currentUser.isLoading} skeleton={<StaticBackground />}>
            <Main>
                <div className="columns-1 sm:columns-2 lg:columns-3 gap-4 space-y-3 w-full">
                    <div className="break-inside-avoid">
                        <ControlledWidget
                            isLoading={projectTechnologyLoading}
                            items={lstProjectTechnologies} 
                            header={{title: 'Project', icon: Folder}}
                            bar={{title: 'Used technologies', groupBy: {lstTechnologies: 'name'}}}
                            pie={{title:'Technology Overview', groupBy: {lstTechnologies: 'name'}}}
                            list={[
                                {leftKey: 'title', size:'lg'},
                                {leftKey: 'isFeatured'}
                            ]}
                            create={{subTitle: 'Add Project & technologis', form: <ProjectTechnologyForm />}}
                            update={{subTitle: 'Update Project & technologies', form: <ProjectTechnologyForm />}}
                            del={{subTitle: 'Delete Project', message: 'Are you sure?', onDelete: handleProjectDelete }}
                            details={[
                                {leftKey: 'title', size:'lg'},
                                {leftKey: 'liveLink', icon: Link},
                                {leftKey: 'sourceCode', icon: SearchCodeIcon},
                                {leftKey: {lstTechnologies: 'name'}, icon: WandSparklesIcon},
                                {leftKey: 'description', size: 'sm'}
                            ]}
                            onSort={debouncedSortProject}
                        />
                    </div>
                    <div className="break-inside-avoid">
                        <ControlledWidget
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
                        <ControlledWidget 
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
        </WithSkeleton>
        </>
    );
}

