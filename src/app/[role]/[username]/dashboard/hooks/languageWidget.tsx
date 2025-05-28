import { CahrtEntry, WidgetCardProps } from "@/components/ui/widget/types";
import { useAppSelector } from "@/lib/store/hooks";
import { BadgePercent, Languages, ListPlusIcon } from "lucide-react";
import UserLanguageForm from "../../forms/userLanguageForm";

export const useLanguageWidget = (): WidgetCardProps => {

    const { loading: languageLoading, lstUserLanguages } = useAppSelector(state => state.userLanguage);
    
    const levelMap: Record<string, number> = {
        native: 100,
        advanced: 80,
        intermediate: 60,
        beginner: 40,
        basic: 20
    };

    const customBarData = (lstUserLanguages as any).map((item: any): CahrtEntry => ({
        name: item.language.name,
        value: levelMap[item.languageProficiency.level.toLowerCase()] ?? 0
    }));

    return {
        isLoading: languageLoading,
        items: lstUserLanguages,
        header: { title: 'Language', icon: Languages },
        radar: { title: 'Language proficiency overview (100%)', customData: customBarData },
        list: [
            { leftKey: {language: 'name'}, size: 'lg' },
            { leftKey: {languageProficiency: 'level'}, icon: BadgePercent }
        ],
        create: { subTitle: 'Modify Languages', form: <UserLanguageForm />, icon: ListPlusIcon},
    }
}