const fs = require('fs');
let text = fs.readFileSync('src/pages/LancamentosPage.tsx', 'utf8');

// The code block to remove from inside LancamentoModal
const oldInputField = `    const InputField = ({ label, name, type = "number", required = true }: any) => (
        <div className="space-y-1.5 flex-1 min-w-[200px]">
            <label className="text-[11px] font-black uppercase tracking-widest text-neutral-500 ml-1">{label}</label>
            <input
                type={type} required={required} name={name} step="any"
                value={formData[name] || ''} onChange={handleChange}
                className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800/80 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none focus:ring-2 focus:ring-blue-500/30 font-bold transition-all"
            />
        </div>
    );`;

text = text.replace(oldInputField, '');

const newInputField = `
const InputField = ({ label, name, type = "number", required = true, value, onChange }: any) => (
    <div className="space-y-1.5 flex-1 min-w-[200px]">
        <label className="text-[11px] font-black uppercase tracking-widest text-neutral-500 ml-1">{label}</label>
        <input
            type={type} required={required} name={name} step="any"
            value={value || ''} onChange={onChange}
            className="w-full px-4 py-3 bg-neutral-50 dark:bg-neutral-800/80 border border-neutral-200 dark:border-neutral-700 rounded-xl outline-none focus:ring-2 focus:ring-blue-500/30 font-bold transition-all"
        />
    </div>
);

`;

text = text.replace('function LancamentoModal({', newInputField + 'function LancamentoModal({');

// Replace all occurrences of <InputField ... />
text = text.replace(/<InputField\s+label="([^"]+)"\s+name="([^"]+)"([^>]*)>/g, '<InputField label="$1" name="$2" value={formData["$2"]} onChange={handleChange} $3>');

fs.writeFileSync('src/pages/LancamentosPage.tsx', text);
console.log('Fixed');
