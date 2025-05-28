import { WidgetCardProps } from "@/components/ui/widget/types";
import { useAppSelector } from "@/lib/store/hooks";
import { Folder, Link, SearchCodeIcon, WandSparklesIcon } from "lucide-react";
import ProjectTechnologyForm from "../../forms/projectTechnologyForm";
import { useDebouncedSortProject, useHandleProjectDelete } from "../handlers";

export const useProjectWidget = (): WidgetCardProps => {
    
    const { loading: projectTechnologyLoading, lstProjectTechnologies } = useAppSelector(state => state.projectTechnology);
    const handleProjectDelete = useHandleProjectDelete();
    const debouncedSortProject = useDebouncedSortProject();

    return {
        isLoading: projectTechnologyLoading,
        items: lstProjectTechnologies,
        header: { title: 'Project', icon: Folder },
        radar: { title: 'Technology proficiency overview (count)', groupBy: { lstTechnologies: 'name' } },
        pie: { title: 'Projects Overview', groupBy: 'title' },
        list: [
            { leftKey: 'title', size: 'lg' },
            { leftKey: 'isFeatured' }
        ],
        create: { subTitle: 'Add Project & technologis', form: <ProjectTechnologyForm /> },
        update: { subTitle: 'Update Project & technologies', form: <ProjectTechnologyForm /> },
        del: { subTitle: 'Delete Project', message: 'Are you sure?', onDelete: handleProjectDelete },
        details: [
            { leftKey: 'title', size: 'lg' },
            { leftKey: 'liveLink', icon: Link },
            { leftKey: 'sourceCode', icon: SearchCodeIcon },
            { leftKey: { lstTechnologies: 'name' }, icon: WandSparklesIcon },
            { leftKey: 'description', size: 'sm' }
        ],
        onSort: debouncedSortProject
    }
}