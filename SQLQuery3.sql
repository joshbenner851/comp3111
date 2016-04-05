select firstName, lastName, email, HKIDPassportNumber 
from Client
where accountNumber in
(select accountNumber from SecurityHolding
where code='22' AND type='stock')
order by lastName ASC;