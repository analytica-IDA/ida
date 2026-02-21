export const maskCPF = (value: string) => {
  return value
    .replace(/\D/g, '')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})/, '$1-$2')
    .replace(/(-\d{2})\d+?$/, '$1');
};

export const maskCNPJ = (value: string) => {
  return value
    .replace(/\D/g, '')
    .replace(/(\d{2})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1/$2')
    .replace(/(\d{4})(\d{1,2})/, '$1-$2')
    .replace(/(-\d{2})\d+?$/, '$1');
};

export const maskPhone = (value: string) => {
  let x = value.replace(/\D/g, '');
  
  // If the user types the 55 prefix or if it's there from previous unmasking, 
  // we treat digits after it as the actual number to avoid doubling +55 (+55 (55) ...)
  if (x.startsWith('55') && x.length > 2) {
    x = x.substring(2);
  }
  
  if (x.length > 11) x = x.substring(0, 11);
  
  let masked = '';
  if (x.length > 0) masked = '+55 (' + x.substring(0, 2);
  if (x.length > 2) masked += ') ' + x.substring(2, 7);
  if (x.length > 7) {
    // Handle both 10 and 11 digits (9999-9999 or 99999-9999)
    // The user specifically asked for +55 (99) 99999-9999
    masked += '-' + x.substring(7, 11);
  }
  
  return masked;
};

export const unmask = (value: string) => {
  return value.replace(/\D/g, '');
};
