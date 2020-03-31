CLASS DummyClass

PROTECT _DummyA AS dword 

// This should be extracted as AsProperty[Get] and AsProperty[Set]
PROPERTY AsProperty AS dword 
	GET
		local a as dword
		a := _DummyA
		RETURN a
	END GET
	SET 
		a := VALUE
		a := VALUE
		RETURN
	END SET
END PROPERTY

// This should not be extracted
PROPERTY AsAutoProperty AS Dword AUTO  

STATIC OPERATOR IMPLICIT ( aa AS DummyClass) AS OBJECT
  local a as dword
  a := 1
  RETURN null 

ACCESS AsAccess
  local a as dword
  a := 1
  return a

ASSIGN AsAssign(newA)
  local a as dword
  a := newA
  return a

METHOD AsMethod() as void strict
  local a as dword
  a := 1
  return 

CONSTRUCTOR()
  local a as dword
  a := 1
  return 

DESTRUCTOR()
  local a as dword
  a := 1
  return 

END CLASS

FUNCTION AsFunction()
  local a as dword
  a := 1
  return a

PROCEDURE AsProcedure()
  local a as dword
  a := 1
  return