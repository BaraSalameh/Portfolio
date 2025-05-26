import { WidgetCardProps } from "@/components/ui/widget/types";
import { useAppSelector } from "@/lib/store/hooks";
import { BadgePercent, Languages } from "lucide-react";
import { useDebouncedSortLanguage, useHandleLanguageDelete } from "../handlers";
import UserLanguageForm from "../../forms/userLanguageForm";

export const useLanguageWidget = (): WidgetCardProps => {

    const { loading: languageLoading, lstUserLanguages } = useAppSelector(state => state.userLanguage);
    const handleLanguageDelete = useHandleLanguageDelete();
    const debouncedSortLanguage = useDebouncedSortLanguage();
    
    return {
        isLoading: languageLoading,
        items: lstUserLanguages,
        header: { title: 'Language', icon: Languages },
        bar: { groupBy: {languageProficiency: 'level'}, title: 'Proficiency' },
        pie: { title: 'Languages Overview', groupBy: {language: 'name'} },
        list: [
            { leftKey: {language: 'name'}, size: 'lg' },
            { leftKey: {languageProficiency: 'level'}, icon: BadgePercent }
        ],
        create: { subTitle: 'Add Language', form: <UserLanguageForm /> },
        update: { subTitle: 'Update Language', form: <UserLanguageForm /> },
        del: { subTitle: 'Delete Project', message: 'Are you sure?', onDelete: handleLanguageDelete },
        details: [
            { leftKey: {language: 'name'}, size: 'lg' },
            { leftKey: {languageProficiency: 'level'}, icon: BadgePercent }
        ],
        onSort: debouncedSortLanguage
    }
}