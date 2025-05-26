import { useFieldArray, useFormContext, Controller } from "react-hook-form";
import Select from "react-select";

export function LanguageFieldArray({ value, onChange, error }: any) {
  const { control } = useFormContext();

  const { fields, append, remove } = useFieldArray({
    control,
    name: "lstLanguages",
  });

  const languageOptions = [
    { id: "1", name: "English" },
    { id: "2", name: "French" },
  ];
  const proficiencyOptions = [
    { id: "a", level: "Beginner" },
    { id: "b", level: "Fluent" },
  ];

  return (
    <div className="space-y-4">
      {fields.map((field, index) => (
        <div key={field.id} className="p-4 border rounded">
          <Controller
            name={`lstLanguages.${index}.lkp_language`}
            control={control}
            render={({ field }) => (
              <Select
                {...field}
                options={languageOptions}
                getOptionLabel={(opt) => opt.name}
                getOptionValue={(opt) => opt.id}
                onChange={field.onChange}
                value={field.value}
              />
            )}
          />

          <Controller
            name={`lstLanguages.${index}.lkp_languageProficiency`}
            control={control}
            render={({ field }) => (
              <Select
                {...field}
                options={proficiencyOptions}
                getOptionLabel={(opt) => opt.level}
                getOptionValue={(opt) => opt.id}
                onChange={field.onChange}
                value={field.value}
              />
            )}
          />

          <button type="button" onClick={() => remove(index)}>
            Remove
          </button>
        </div>
      ))}

      <button
        type="button"
        onClick={() =>
          append({
            lkp_language: null,
            lkp_languageProficiency: null,
          })
        }
      >
        Add Language
      </button>

      {error && <p className="text-red-600 text-sm">{error.message}</p>}
    </div>
  );
}
