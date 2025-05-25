import { WidgetCardProps } from "@/components/ui/widget/types";
import { useAppSelector } from "@/lib/store/hooks";
import { Briefcase, Clock, LocationEdit } from "lucide-react";
import { useDebouncedSortExperience, useHandleExperienceDelete } from "../handlers";
import ExperienceForm from "../../forms/experienceForm";

export const useExperienceWidget = (): WidgetCardProps => {

    const { loading: experienceLoading, lstExperiences } = useAppSelector(state => state.experience);
    const handleExperienceDelete = useHandleExperienceDelete();
    const debouncedSortExperience = useDebouncedSortExperience();
    
    return {
        isLoading: experienceLoading,
        items: lstExperiences,
        header: { title: 'Experience', icon: Briefcase },
        bar: { groupBy: 'jobTitle' },
        pie: { title: 'Experience Overview', groupBy: 'jobTitle' },
        list: [
            { leftKey: 'jobTitle', between: 'at', rightKey: 'companyName', size: 'lg' },
            { leftKey: 'location', icon: LocationEdit },
            { leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true }
        ],
        create: { subTitle: 'Add Experience', form: <ExperienceForm /> },
        update: { subTitle: 'Update Experience', form: <ExperienceForm /> },
        del: { subTitle: 'Delete Experience', message: 'Are you sure?', onDelete: handleExperienceDelete },
        details: [
            { leftKey: 'jobTitle', between: 'at', rightKey: 'companyName', size: 'lg' },
            { leftKey: 'location', icon: LocationEdit },
            { leftKey: 'startDate', between: '-', rightKey: 'endDate', icon: Clock, isTime: true },
            { leftKey: 'description', size: 'sm' }
        ],
        onSort: debouncedSortExperience
    }
}