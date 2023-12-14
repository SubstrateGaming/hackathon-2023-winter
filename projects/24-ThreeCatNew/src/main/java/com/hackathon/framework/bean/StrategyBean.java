package com.hackathon.framework.bean;

import java.util.List;
import java.util.Map;

/**
 * 读取yaml里面的策略
 */
public class StrategyBean {

    private String compile;

    private String compiledNames;

    private List<String> directory;

    private String sdkName;

    public List<String> getDirectory() {
        return directory;
    }

    public void setDirectory(List<String> directory) {
        this.directory = directory;
    }

    public String getCompile() {
        return compile;
    }

    public void setCompile(String compile) {
        this.compile = compile;
    }

    public String getCompiledNames() {
        return compiledNames;
    }

    public void setCompiledNames(String compiled) {
        this.compiledNames = compiled;
    }

    public String getSdkName() {
        return sdkName;
    }

    public void setSdkName(String sdkName) {
        this.sdkName = sdkName;
    }

    public String getAssertionPath() {
        return assertionPath;
    }

    public void setAssertionPath(String assertionPath) {
        this.assertionPath = assertionPath;
    }

    public Boolean getEnableCoverageTarget() {
        return enableCoverageTarget;
    }

    public void setEnableCoverageTarget(Boolean enableCoverageTarget) {
        this.enableCoverageTarget = enableCoverageTarget;
    }

    public Float getCoverageThreshold() {
        return coverageThreshold;
    }

    public void setCoverageThreshold(Float coverageThreshold) {
        this.coverageThreshold = coverageThreshold;
    }

    public Float getCoreFileThreshold() {
        return coreFileThreshold;
    }

    public void setCoreFileThreshold(Float coreFileThreshold) {
        this.coreFileThreshold = coreFileThreshold;
    }

    public String getCISelector() {
        return CISelector;
    }

    public void setCISelector(String CISelector) {
        this.CISelector = CISelector;
    }

    public Map<String, Object> getScanStages() {
        return scanStages;
    }

    public void setScanStages(Map<String, Object> scanStages) {
        this.scanStages = scanStages;
    }

    public List<String> getScanWeight() {
        return scanWeight;
    }

    public void setScanWeight(List<String> scanWeight) {
        this.scanWeight = scanWeight;
    }

    public String getReportMode() {
        return reportMode;
    }

    public void setReportMode(String reportMode) {
        this.reportMode = reportMode;
    }

    private String assertionPath;

    private Boolean enableCoverageTarget;

    private Float coverageThreshold;

    private Float coreFileThreshold;

    private String CISelector;

    private Map<String,Object> scanStages;

    private List<String> scanWeight;

    private String reportMode;

    public StrategyBean(){

    }
}
