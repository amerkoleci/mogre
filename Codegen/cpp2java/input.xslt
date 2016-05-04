<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:include href="helpers.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="iso-8859-1" indent="yes"/>
	<!-- ############################## MAIN - calls namespace ############################## -->
	<xsl:template match="/">
		<xsl:element name="meta">
			<xsl:for-each select="doxygen/compounddef[@kind='namespace']">
				<xsl:call-template name="namespace"/>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
	<!-- ############### NAMESPACE - calls class; globals - calls typedef, enum, function, variable ################# -->
	<!-- cursor on doxygen/compounddef -->
	<xsl:template name="namespace">
		<xsl:element name="namespace">
			<xsl:attribute name="name">
				<xsl:value-of select="compoundname" />
			</xsl:attribute>
			<xsl:variable name="name" select="compoundname"/>
			<xsl:if test="contains($name,'::')">
				<xsl:attribute name="name">
					<xsl:value-of select="substring-before($name,'::')" />
				</xsl:attribute>
				<xsl:attribute name="second">
					<xsl:value-of select="substring-after($name,'::')" />
				</xsl:attribute>
			</xsl:if>
			<!-- classes -->
			<xsl:for-each select="innerclass">
				<xsl:call-template name="class">
					<xsl:with-param name="refid" select="@refid"/>
				</xsl:call-template>
			</xsl:for-each>
			<!-- global typedefs -->
			<xsl:for-each select="sectiondef[@kind='typedef']">
				<xsl:call-template name="typedef"/>
			</xsl:for-each>
			<!-- global enums -->
			<xsl:for-each select="sectiondef[@kind='enum']">
				<xsl:call-template name="enumeration"/>
			</xsl:for-each>
			<!-- global functions -->
			<xsl:for-each select="sectiondef[@kind='func']">
				<xsl:if test="not(memberdef/templateparamlist)">
					<xsl:call-template name="function"/>
				</xsl:if>
			</xsl:for-each>
			<!-- global variables -->
			<xsl:for-each select="sectiondef[@kind='var']">
				<xsl:call-template name="variable"/>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
	<!-- ############### CLASS - calls struct, derivation, enumeration, typedef, function ################## -->
	<!-- cursor on doxygen/compounddef/innerclass -->
	<xsl:template name="class">
		<xsl:param name="refid"/>
		<!-- structs -->
		<xsl:for-each select="/doxygen/compounddef[@id=$refid and @kind='struct']/sectiondef[@kind='public-attrib']">
			<xsl:call-template name="struct"/>
		</xsl:for-each>
		<!-- classes -->
		<xsl:for-each select="/doxygen/compounddef[@id=$refid and @kind='class']">
			<xsl:element name="class">
				<xsl:attribute name="name">
					<xsl:value-of select="substring-after(compoundname,'::')"/>
				</xsl:attribute>
				<xsl:attribute name="fullName">
					<xsl:value-of select="compoundname"/>
				</xsl:attribute>
				<xsl:attribute name="protection">
					<xsl:value-of select="@prot"/>
				</xsl:attribute>
				<xsl:attribute name="includeFile">
					<xsl:value-of select="substring-after(location/@file,'/include/')"/>
				</xsl:attribute>
				
				<!-- template attributes -->
				<xsl:if test="templateparamlist">
					<xsl:attribute name="template">
						<xsl:value-of select="'true'"/>
					</xsl:attribute>
					<xsl:attribute name="templateType">
						<xsl:value-of select="templateparamlist/param/type"/>
					</xsl:attribute>
					<xsl:attribute name="templateDeclaration">
						<xsl:value-of select="templateparamlist/param/declname" />
					</xsl:attribute>
					<xsl:for-each select="inheritancegraph/node">
						<xsl:if test="link/@refid=$refid">
							<xsl:variable name="childnode" select="childnode/@refid"/>
							<xsl:for-each select="../node">
								<xsl:if test="@id=$childnode">
									<xsl:attribute name="templateName"><xsl:variable name="string" select="substring-before(label,' &gt;')"/><xsl:value-of select="substring-after($string,'&lt; ')"/></xsl:attribute>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<!-- getting the right name of inner classes -->
				<xsl:variable name="name">
					<xsl:call-template name="substring-after-last">
						<xsl:with-param name="string" select="compoundname" />
						<xsl:with-param name="delimiter" select="'::'" />
					</xsl:call-template>
				</xsl:variable>
				<xsl:attribute name="name">
					<xsl:value-of select="$name"/>
				</xsl:attribute>
				<xsl:element name="summary">
					<xsl:value-of select="normalize-space(briefdescription)"/>
				</xsl:element>
				<!-- cursor on doxygen/compounddef[@kind="class"] -->
				<!-- derivation -->
				<xsl:call-template name="derivation">
					<xsl:with-param name="refid" select="$refid"/>
				</xsl:call-template>
				<!-- enumerations -->
				<xsl:for-each select="sectiondef[@kind='public-type' or @kind='protected-type']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='enum']">
							<xsl:call-template name="enumeration"/>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				<!-- typedefs -->
				<xsl:for-each select="sectiondef[@kind='public-type' or @kind='protected-type' or @kind='private-type']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='typedef']">
							<xsl:call-template name="typedef"/>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				<!-- public functions -->
				<xsl:for-each select="sectiondef[@kind='public-func']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='function']">
							<xsl:call-template name="function">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- static functions -->
				<xsl:for-each select="sectiondef[@kind='public-static-func']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='function']">
							<xsl:call-template name="function">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				<!-- recursion for inner classes -->
				<xsl:for-each select="innerclass">
					<xsl:call-template name="class">
						<xsl:with-param name="refid" select="@refid"/>
					</xsl:call-template>
				</xsl:for-each>
				
				<!-- static variables -->
				<xsl:for-each select="sectiondef[@kind='public-static-attrib']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='variable']">
							<xsl:call-template name="variable">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- public variables -->
				<xsl:for-each select="sectiondef[@kind='public-attrib']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='variable']">
							<xsl:call-template name="variable">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- protected functions -->
				<xsl:for-each select="sectiondef[@kind='protected-func']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='function']">
							<xsl:call-template name="function">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- protected variables -->
				<xsl:for-each select="sectiondef[@kind='protected-attrib']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='variable']">
							<xsl:call-template name="variable">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- protected static functions -->
				<xsl:for-each select="sectiondef[@kind='protected-static-func']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='function']">
							<xsl:call-template name="function">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
				<!-- protected static variables -->
				<xsl:for-each select="sectiondef[@kind='protected-static-attrib']">
					<xsl:choose>
						<xsl:when test="memberdef[@kind='variable']">
							<xsl:call-template name="variable">
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:for-each>
				
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<!-- ##################### DERIVATION ######################### -->
	<!-- cursor on doxygen/compounddef -->
	<xsl:template name="derivation">
		<xsl:param name="refid"/>
		<xsl:if test="basecompoundref">
			<xsl:element name="inherits">
                                
				<xsl:if test="basecompoundref[@refid='classCLRObject']">
					<xsl:element name="baseClass">
						<xsl:value-of select="'CLRObject'"/>
					</xsl:element>
				</xsl:if>
				
				<xsl:for-each select="inheritancegraph/node[link/@refid=$refid]">
					<xsl:for-each select="childnode">
						<xsl:variable name="childid" select="@refid"/>
						<xsl:variable name="baseClass" select="substring-after(../../node[@id=$childid]/label,'::')"/>
						<!-- non templates first -->
						<xsl:if test="not(contains($baseClass,'&lt;'))">
							<xsl:element name="baseClass">
								<xsl:value-of select="$baseClass"/>
							</xsl:element>
						</xsl:if>
					</xsl:for-each>
					<xsl:for-each select="childnode">
						<xsl:variable name="childid" select="@refid"/>
						<xsl:variable name="baseClass" select="substring-after(../../node[@id=$childid]/label,'::')"/>
						<!-- templates -->
						<xsl:if test="contains($baseClass,'&lt;')">
							<xsl:element name="baseClass">
								<xsl:value-of select="$baseClass"/>
							</xsl:element>
						</xsl:if>
					</xsl:for-each>
				</xsl:for-each>
			</xsl:element>
		</xsl:if>
	</xsl:template>
	<!-- ##################### ENUMERATION ######################### -->
	<!-- cursor on doxygen/compounddef/sectiondef -->
	<xsl:template name="enumeration">
		<xsl:for-each select="memberdef[@kind='enum']">
			<!-- test if enum belongs to actual class -->
			<xsl:element name="enumeration">
				<xsl:attribute name="name">
					<xsl:value-of select="name"/>
				</xsl:attribute>
				<xsl:attribute name="protection">
					<xsl:value-of select="@prot"/>
				</xsl:attribute>
				<xsl:attribute name="includeFile">
					<xsl:value-of select="substring-after(location/@file,'/include/')"/>
				</xsl:attribute>
				
				<xsl:for-each select="enumvalue">
					<xsl:element name="enum">
						<xsl:attribute name="name">
							<xsl:value-of select="name"/>
						</xsl:attribute>
						<xsl:if test="normalize-space(briefdescription)!=''">
							<xsl:element name="summary">
								<xsl:value-of select="normalize-space(briefdescription)"/>
							</xsl:element>
						</xsl:if>
					</xsl:element>
				</xsl:for-each>
				<xsl:if test="normalize-space(briefdescription)!=''">
					<xsl:element name="summary">
						<xsl:value-of select="normalize-space(briefdescription)"/>
					</xsl:element>
				</xsl:if>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<!-- ##################### TYPEDEF - calls typeMap ######################### -->
	<!-- cursor on doxygen/compounddef/sectiondef -->
	<xsl:template name="typedef">
		<xsl:for-each select="memberdef[@kind='typedef']">
			<xsl:element name="typedef">
				<xsl:attribute name="name">
					<xsl:value-of select="name"/>
				</xsl:attribute>
				<xsl:attribute name="protection">
					<xsl:value-of select="@prot"/>
				</xsl:attribute>
				<xsl:attribute name="includeFile">
					<xsl:value-of select="substring-after(location/@file,'/include/')"/>
				</xsl:attribute>
				
				<xsl:choose>
					<xsl:when test="contains(type,'std::') or contains(type,',') or contains(type,'::type') or contains(type, 'HashedVector')">
						<xsl:attribute name="basetype">
							<xsl:value-of select="substring-before(type,'&lt;')"/>
						</xsl:attribute>
						<xsl:variable name="basetype" select="substring-before(type,'&lt;')"/>
						<xsl:if test="contains($basetype,'&#xA;')">
							<xsl:attribute name="basetype">
								<xsl:value-of select="normalize-space(substring-after($basetype,'&#xA;'))"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:variable name="string1" select="substring-after(type,'&lt; ')"/>
						<xsl:choose>
							<!-- e.g. std::multimap< std::pair<size_t, size_t>, std::pair<size_t, size_t> > -->
							<xsl:when test="starts-with($string1,'std::')">
								<xsl:variable name="type1" select="substring-before($string1,', std::')"/>
								<xsl:element name="typedef">
									<xsl:attribute name="basetype">
										<xsl:value-of select="substring-before($type1,'&lt;')"/>
									</xsl:attribute>
									<xsl:variable name="inner_string1" select="substring-after($type1,'&lt; ')"/>
									<xsl:call-template name="typeMap">
										<xsl:with-param name="type" select="normalize-space(substring-before($inner_string1,','))"/>
									</xsl:call-template>
									<xsl:variable name="string2" select="substring-before($inner_string1,' &gt;')"/>
									<xsl:call-template name="typeMap">
										<xsl:with-param name="type" select="normalize-space(substring-after($string2,','))"/>
									</xsl:call-template>
								</xsl:element>
								<xsl:variable name="type2" select="substring-after($string1,'&gt;, ')"/>
								<xsl:element name="typedef">
									<xsl:attribute name="basetype">
										<xsl:value-of select="substring-before($type2,'&lt;')"/>
									</xsl:attribute>
									<xsl:variable name="inner_string1" select="substring-after($type2,'&lt; ')"/>
									<xsl:call-template name="typeMap">
										<xsl:with-param name="type" select="normalize-space(substring-before($inner_string1,','))"/>
									</xsl:call-template>
									<xsl:variable name="string2" select="substring-before($inner_string1,' &gt;')"/>
									<xsl:call-template name="typeMap">
										<xsl:with-param name="type" select="normalize-space(substring-after($string2,','))"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:when>
							<xsl:when test="contains($string1,',')">
								<xsl:call-template name="typeMap">
									<xsl:with-param name="type" select="substring-before($string1,',')"/>
								</xsl:call-template>
								<xsl:variable name="string2" select="substring-after(type,', ')"/>
								<xsl:choose>
									<!-- e.g. std::map<Vector3, size_t, vectorLess> -->
									<xsl:when test="contains($string2,',')">
										<xsl:call-template name="typeMap">
											<xsl:with-param name="type" select="normalize-space(substring-before($string2,','))"/>
										</xsl:call-template>
									</xsl:when>
									<!-- e.g. std::map<Vector3, size_t> -->
									<xsl:otherwise>
										<xsl:call-template name="typeMap">
											<xsl:with-param name="type" select="substring-before($string2,'&gt;')"/>
										</xsl:call-template>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="typeMap">
									<xsl:with-param name="type" select="substring-before($string1,'&gt;')"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<!-- iterator, simple typedefs -->
					<xsl:otherwise>
						<xsl:attribute name="basetype">
							<xsl:value-of select="type"/>
						</xsl:attribute>
						<xsl:variable name="basetype" select="type"/>
						<xsl:if test="contains($basetype,'&#xA;')">
							<xsl:attribute name="basetype">
								<xsl:value-of select="normalize-space(substring-after($basetype,'&#xA;'))"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="contains($basetype,'struct ')">
							<xsl:attribute name="basetype">
								<xsl:value-of select="normalize-space(substring-after($basetype,'struct '))"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="type/ref[@kindref='member']">
								<xsl:choose>
									<xsl:when test="contains(type/ref[@kindref='member'],'::')">
										<xsl:attribute name="type">
											<xsl:value-of select="substring-after(type/ref[@kindref='member'],'::')"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:attribute name="type">
											<xsl:value-of select="type/ref[@kindref='member']"/>
										</xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="type/ref[@kindref='compound']">
								<xsl:choose>
									<xsl:when test="count(type/ref[@kindref='compound'])=2">
										<xsl:attribute name="type">
											<xsl:value-of select="type/ref[@kindref='compound' and position()=2]"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="string1" select="substring-after(type,'&lt;')"/>
										<xsl:attribute name="type">
											<xsl:value-of select="normalize-space(substring-before($string1,'&gt;'))"/>
										</xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<!-- ##################### STRUCT - calls typeMap ######################### -->
	<!-- cursor on doxygen/compounddef/sectiondef -->
	<xsl:template name="struct">
		<xsl:element name="struct">
			<xsl:attribute name="name">
				<xsl:value-of select="substring-after(../compoundname,'::')"/>
			</xsl:attribute>
			<xsl:variable name="name" select="substring-after(../compoundname,'::')"/>
			<xsl:if test="contains($name,'::')">
				<xsl:attribute name="name">
					<xsl:value-of select="substring-after($name,'::')"/>
				</xsl:attribute>
			</xsl:if>
			<!-- template attributes -->
			<xsl:if test="../templateparamlist">
				<xsl:variable name="refid" select="../@id"/>
				<xsl:attribute name="template">
					<xsl:value-of select="'true'"/>
				</xsl:attribute>
				<xsl:attribute name="templateType">
					<xsl:value-of select="../templateparamlist/param/type"/>
				</xsl:attribute>
				<xsl:attribute name="templateDeclaration">
					<xsl:value-of  select="../templateparamlist/param/declname"/>
				</xsl:attribute>
				<xsl:for-each select="../collaborationgraph/node">
					<xsl:if test="link/@refid=$refid">
						<xsl:variable name="childnode" select="childnode/@refid"/>
						<xsl:for-each select="../node">
							<xsl:if test="@id=$childnode">
								<xsl:attribute name="templateName">
									<xsl:value-of select="label"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>
			</xsl:if>
			<xsl:attribute name="protection">
				<xsl:value-of select="../@prot"/>
			</xsl:attribute>
			<xsl:attribute name="includeFile">
				<xsl:value-of select="../includes"/>
			</xsl:attribute>
			
			<xsl:element name="summary">
				<xsl:value-of select="normalize-space(briefdescription)"/>
			</xsl:element>
			
			<!-- struct attributes -->
			<xsl:for-each select="memberdef[@kind='variable']">
				<xsl:element name="variable">
					<xsl:attribute name="protection">
						<xsl:value-of select="@prot"/>
					</xsl:attribute>
					<xsl:attribute name="static">
						<xsl:value-of select="@static"/>
					</xsl:attribute>
					
					<xsl:if test="type and type!='virtual'">
						<xsl:call-template name="type"/>
					</xsl:if>
					<xsl:element name="definition">
						<xsl:value-of select="definition"/>
					</xsl:element>
					
					<xsl:element name="name">
						<xsl:value-of select="name"/>
					</xsl:element>
				</xsl:element>
			</xsl:for-each>
			<!-- struct functions -->
			<xsl:for-each select="../sectiondef[@kind='public-func']">
				<xsl:call-template name="function"/>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
	<!-- ############## FUNCTION - calls type ############## -->
	<!-- cursor on doxygen/compounddef/sectiondef -->
	<xsl:template name="function">
		<xsl:for-each select="memberdef">
			<!-- test if function belongs to actual class -->
			<xsl:if test="starts-with(@id,../../@id)">
				<xsl:choose>
					<!-- destructor - do nothing -->
					<xsl:when test="starts-with(name,'~')"/>
					<xsl:otherwise>
						<xsl:element name="function">
							<xsl:attribute name="virt">
								<xsl:value-of select="@virt"/>
							</xsl:attribute>
							<xsl:attribute name="protection">
								<xsl:value-of select="@prot"/>
							</xsl:attribute>
							<xsl:attribute name="static">
								<xsl:value-of select="@static"/>
							</xsl:attribute>
							<xsl:attribute name="const">
								<xsl:choose>
									<xsl:when test="@const='yes'">
										<xsl:value-of select="'true'"/>
									</xsl:when>
									<xsl:when test="@const='no'">
										<xsl:value-of select="'false'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:attribute>
							
							<!-- template attributes -->
							<xsl:if test="templateparamlist">
								<xsl:variable name="refid" select="../../@id"/>
								<xsl:attribute name="template">
									<xsl:value-of select="'true'"/>
								</xsl:attribute>
								<xsl:attribute name="templateType">
									<xsl:value-of select="templateparamlist/param/type"/>
								</xsl:attribute>
								<xsl:attribute name="templateDeclaration">
									<xsl:value-of select="templateparamlist/param/declname"/>
								</xsl:attribute>
								<xsl:for-each select="inheritancegraph/node">
									<xsl:if test="link/@refid=$refid">
										<xsl:variable name="childnode" select="childnode/@refid"/>
										<xsl:for-each select="../node">
											<xsl:if test="@id=$childnode">
												<xsl:attribute name="templateName"><xsl:variable name="string" select="substring-before(label,' &gt;')"/><xsl:value-of select="substring-after($string,'&lt; ')"/></xsl:attribute>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</xsl:if>
							<!-- type of the function -->
							<xsl:if test="type and type!='virtual'">
								<xsl:call-template name="type"/>
							</xsl:if>
							<xsl:element name="definition">
								<xsl:value-of select="definition"/>
							</xsl:element>
							<!-- name of the function -->
							<xsl:element name="name">
								<xsl:choose>
									<xsl:when test="contains(name,'::')">
										<xsl:value-of select="substring-after(name,'::')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="name"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:element>
							<xsl:if test="normalize-space(briefdescription)!=''">
								<xsl:element name="summary">
									<xsl:value-of select="normalize-space(briefdescription)"/>
								</xsl:element>
							</xsl:if>
							<!-- parameter of the function -->
							<xsl:if test="param/type!='' or param/declname!=''">
								<xsl:element name="parameters">
									<xsl:for-each select="param">
										<xsl:element name="parameter">
											<xsl:call-template name="type"/>
											
											<xsl:if test="defval!=''">
												<xsl:element name="defval">
													<xsl:value-of select="defval"/>
												</xsl:element>
											</xsl:if>
											<xsl:variable name="paramname" select="declname"/>
											<xsl:variable name="paramsummary" select="normalize-space(../detaileddescription//parameteritem[parameternamelist/parametername=$paramname]/parameterdescription)"/>
											<xsl:if test="$paramsummary!=''">
												<xsl:element name="summary">
													<xsl:value-of select="$paramsummary"/>
												</xsl:element>
											</xsl:if>
										</xsl:element>
									</xsl:for-each>
								</xsl:element>
							</xsl:if>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<!-- ##################### TYPE - calls typeMap ##################### -->
	<!-- cursor on doxygen/compounddef/sectiondef/memberdef/param -->
	<xsl:template name="type">
		<xsl:choose>
			<!-- passed by reference -->
			<xsl:when test="contains(type,'&amp;')">
				<xsl:attribute name="passedBy">
					<xsl:value-of select="'reference'"/>
				</xsl:attribute>
				<xsl:choose>
					<!-- current class is a template -->
					<xsl:when test="../../templateparamlist or ../../../templateparamlist">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="substring-before(type,' &amp;')"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:when test="type/ref">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type">
								<xsl:call-template name="implode">
									<xsl:with-param name="items" select="type/ref" />
									<xsl:with-param name="separator" select="' '"/>
								</xsl:call-template>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="substring-before(type,' &amp;')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- passed by pointer -->
			<xsl:when test="contains(type,'*')">
				<xsl:choose>
					<xsl:when test="contains(type,'**')">
						<xsl:attribute name="passedBy">
							<xsl:value-of select="'PointerPointer'"/>
						</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="passedBy">
							<xsl:value-of select="'pointer'"/>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:variable name="type" select="substring-before(type,' *')"/>
				<xsl:choose>
					<!-- current class is a template -->
					<xsl:when test="../../templateparamlist or ../../../templateparamlist">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="substring-before(type,' *')"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:when test="type/ref">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type">
								<xsl:call-template name="implode">
									<xsl:with-param name="items" select="type/ref" />
									<xsl:with-param name="separator" select="' '"/>
								</xsl:call-template>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="substring-before(type,' *')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- passed by value -->
			<xsl:otherwise>
				<xsl:attribute name="passedBy">
					<xsl:value-of select="'value'"/>
				</xsl:attribute>
				<xsl:choose>
					<!-- current class is a template -->
					<xsl:when test="../../templateparamlist or ../../../templateparamlist">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="type"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:when test="count(type/ref)=1">
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type">
								<xsl:value-of select="type/ref"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="typeMap">
							<xsl:with-param name="type" select="type"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
		<!-- name -->
		<xsl:if test="declname!=''">
			<xsl:element name="name">
				<xsl:value-of select="declname"/>
			</xsl:element>
		</xsl:if>
	</xsl:template>
	<!-- ######################## TYPEMAP ######################## -->
	<!-- cursor on doxygen/compounddef/sectiondef/memberdef/param -->
	<xsl:template name="typeMap">
		<xsl:param name="type"/>
		<xsl:if test="type!=''">
			<xsl:element name="type">
				<xsl:if test="starts-with(type,'const')">
					<xsl:attribute name="const">
						<xsl:value-of select="'true'"/>
					</xsl:attribute>
				</xsl:if>
				
				<!-- implement custom ogre container (vector replace std::vector) -->
				<xsl:if test="contains(type,'vector')">
					<xsl:attribute name="container">
						<xsl:value-of select="'vector'"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="contains(type,'HashedVector')">
					<xsl:attribute name="container">
						<xsl:value-of select="'HashedVector'"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="contains(type,'list')">
					<xsl:attribute name="container">
						<xsl:value-of select="'list'"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="contains(type,'map')">
					<xsl:attribute name="container">
						<xsl:value-of select="'map'"/>
					</xsl:attribute>
					<xsl:attribute name="containerKey">
						<xsl:value-of select="substring-before(substring-after(type,'&lt; '),',')"/>
					</xsl:attribute>
					<xsl:attribute name="containerValue">
						<xsl:value-of select="substring-before(substring-after(type,', '),' &gt;')"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="contains(type,'pair')">
					<xsl:attribute name="container">
						<xsl:value-of select="'pair'"/>
					</xsl:attribute>
					<xsl:attribute name="containerKey">
						<xsl:value-of select="substring-before(substring-after(type,'&lt; '),',')"/>
					</xsl:attribute>
					<xsl:attribute name="containerValue">
						<xsl:value-of select="substring-before(substring-after(type,', '),' &gt;')"/>
					</xsl:attribute>
				</xsl:if>
				
				<xsl:if test="array">
					<xsl:attribute name="array">
						<xsl:value-of select="array"/>
					</xsl:attribute>
				</xsl:if>
				
				<xsl:choose>
					<!-- passed by reference -->
					<xsl:when test="contains($type,'&amp;')">
						<xsl:attribute name="passedBy">
							<xsl:value-of select="'reference'"/>
						</xsl:attribute>
						<xsl:value-of select="substring-before($type,' &amp;')"/>
					</xsl:when>
					<!-- passed by pointer -->
					<xsl:when test="contains($type,'*')">
						<xsl:choose>
							<xsl:when test="contains($type,'**')">
								<xsl:attribute name="passedBy">
									<xsl:value-of select="'PointerPointer'"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="passedBy">
									<xsl:value-of select="'pointer'"/>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="substring-before($type,' *')"/>
					</xsl:when>
					<!-- MOGRE 1.7 change implement custom ogre container -->
					<xsl:when test="contains($type,'vector')">
						<xsl:value-of select="normalize-space(substring-after($type,'vector'))"/>
					</xsl:when>
					<xsl:when test="contains($type,'HashedVector')">
						<xsl:value-of select="normalize-space(substring-after($type,'HashedVector'))"/>
					</xsl:when>
					<xsl:when test="contains($type,'list')">
						<xsl:value-of select="normalize-space(substring-after($type,'list'))"/>
					</xsl:when>
					<xsl:when test="contains($type,'map')">
						<xsl:value-of select="normalize-space(substring-after($type,'map'))"/>
					</xsl:when>
					<xsl:when test="contains($type,'pair')">
						<xsl:value-of select="normalize-space(substring-after($type,'pair'))"/>
					</xsl:when>
					<!-- MOGRE 1.7-->
					
					<!-- passed by value -->
					<xsl:otherwise>
						<xsl:value-of select="$type"/>
					</xsl:otherwise>
				</xsl:choose>
				
				<!-- IT CONTAINED THE TYPEMAPS (Real -> float etc.) -->
				
			</xsl:element>
		</xsl:if>
	</xsl:template>
	<!-- ############ VARIABLE ############ -->
	<!-- cursor on doxygen/compounddef/sectiondef -->
	<xsl:template name="variable">
		<xsl:for-each select="memberdef">
			<xsl:if test="not(starts-with(type,'union'))">
				<xsl:element name="variable">
					<xsl:attribute name="protection">
						<xsl:value-of select="@prot"/>
					</xsl:attribute>
					<xsl:attribute name="static">
						<xsl:value-of select="@static"/>
					</xsl:attribute>
					
					<xsl:if test="type and type!='virtual'">
						<xsl:call-template name="type"/>
					</xsl:if>
					<xsl:element name="definition">
						<xsl:value-of select="definition"/>
					</xsl:element>
					<xsl:element name="name">
						<xsl:value-of select="name"/>
					</xsl:element>
					<xsl:if test="normalize-space(briefdescription)!=''">
						<xsl:element name="summary">
							<xsl:value-of select="normalize-space(briefdescription)"/>
						</xsl:element>
					</xsl:if>					
				</xsl:element>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>